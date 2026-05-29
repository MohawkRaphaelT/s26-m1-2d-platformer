using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Use rigidbody to move AI
    public Rigidbody2D rb2d;
    //  Which layers does raycast respect / look for
    public LayerMask layerMask;
    public float distanceCheckWall = 1;
    public float distanceCheckWallOffsetY = -0.5f;
    public float distanceCheckLedge = 1;
    //
    public SpriteRenderer spriteRenderer;
    public float patrolSpeedX = 5;
    public bool moveRight = true;
    //
    public Player player;
    public float playerChaseRadius = 3;
    public float chaseSpeedX = 7;

    void Update()
    {
        // How far is player from AI?
        float distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);
        if (distanceToPlayer <= playerChaseRadius)
        {
            Chase();
        }
        else
        {
            Patrol();
        }

        // Flip on X axis if we are NOT moving right
        spriteRenderer.flipX = !moveRight;
    }

    void Chase()
    {
        // Check to see if player X coordinate is greater than ours = move right
        moveRight = player.transform.position.x > this.transform.position.x;
        // Move at chase speed in direction
        float linearVelocityX = moveRight ? +chaseSpeedX : -chaseSpeedX;
        rb2d.linearVelocityX = linearVelocityX;
    }

    void Patrol()
    {
        // We will shoot ray to detect walls from centre of enemy
        Vector2 wallDetectedOrigin = transform.position;
        // Offset Y up or down for this check
        wallDetectedOrigin.y += distanceCheckWallOffsetY;
        // Ternary operator
        // If we are moving right, direction is right, if left, direction is left
        Vector2 wallDetectDir = moveRight ? Vector2.right : Vector2.left;
        // Shoot ray from origing in direction to a max of distance against layers in layer mask only
        bool willHitWall = Physics2D.Raycast(wallDetectedOrigin, wallDetectDir, distanceCheckWall, layerMask);
        // Debug draw the raycast
        Debug.DrawLine(wallDetectedOrigin, wallDetectedOrigin + wallDetectDir * distanceCheckWall);


        // Calculate position in front of AI to move
        Vector2 ledgeDetectOffsetDir = moveRight ? Vector2.right : Vector2.left;
        Vector2 ledgeDetectOrigin = (Vector2)transform.position + ledgeDetectOffsetDir;
        // Shoot ray downwards eg. off of ledge
        Vector2 ledgeDetectDir = Vector2.down;
        // If raycast DOESN'T hit anything we will walk off ledge
        bool willWalkOffLedge = !Physics2D.Raycast(ledgeDetectOrigin, ledgeDetectDir, distanceCheckLedge, layerMask);
        Debug.DrawLine(ledgeDetectOrigin, ledgeDetectOrigin + ledgeDetectDir * distanceCheckLedge);


        // If we will hit wall or walk off ledge, move in the other direction
        if (willHitWall == true || willWalkOffLedge == true)
        {
            // Move right is NOT what it currently is, invert / flip bool
            moveRight = !moveRight;
        }

        // MOVE!
        // Calculate which direction we need to move in
        //                      boolean   ? if true do  : if false do
        float linearVelocityX = moveRight ? +patrolSpeedX : -patrolSpeedX;
        rb2d.linearVelocityX = linearVelocityX;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            // Reset game? Level? Life reduced?
            Debug.Log("Hit player");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerChaseRadius);
    }
}
