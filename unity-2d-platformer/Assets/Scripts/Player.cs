using UnityEngine;

public class Player : MonoBehaviour
{
    // VARIABLES
    // We want to know about the player's RigidBody2D component to add forces to it
    public Rigidbody2D rb2d;
    //Get the player's collider / shape
    public CapsuleCollider2D capsuleCollider;
    // We want the player's animator component to synchronize its states to player movement
    public Animator animator;
    // We want to be able to control the sprite flipX to align with facing direction when we move
    public SpriteRenderer spriteRenderer;

    // How fast do we want the player to move?
    public float moveSpeed = 10f;
    //
    public float jumpSpeed = 10f;
    public float maxJumpTime = 0.300f; // in seconds
    public float maxCoyoteTime = 0.100f; // in seconds
    public float fallGravity = -10; // y
    //
    public LayerMask groundLayer;
    //
    public float raycastDistance = 0.05f;

    //
    private float coyoteTimeRemaining;
    private float jumpTimeRemaining;


    void Update()
    {
        ///////////////////////////////////////////////////////////////////////////////
        /// MOVE HORIZONTAL

        // Get the player's movement input from Unity's legacy input system
        float moveX = Input.GetAxis("Horizontal");
        // Math.Abs() gives us the number's absolute value
        // eg. Abs(+1) and Abs(-1) both give us +1.
        bool isMovingHorizontally = Mathf.Abs(moveX) > 0.1f;
        if (isMovingHorizontally)
        {
            // move X is positive means we are moving right
            bool isFacingLeft = moveX < 0;
            spriteRenderer.flipX = isFacingLeft;

            // Check to see if player is hitting a wall horizontally
            Vector2 centre = transform.position;
            Vector2 extents = capsuleCollider.bounds.extents;
            float extentsX = isFacingLeft ? -extents.x : +extents.x;
            Vector2 edgeClipTopOrigin = centre + new Vector2(extentsX, +extents.y * 0.95f);
            Vector2 edgeClipBotOrigin = centre + new Vector2(extentsX, -extents.y * 0.85f);
            Vector2 direction = Vector2.Normalize(new Vector2(extentsX, 0));
            Vector2 edgeClipRayDistance = direction * raycastDistance;
            bool hitTop = Physics2D.Raycast(edgeClipTopOrigin, direction, raycastDistance, groundLayer);
            bool hitBot = Physics2D.Raycast(edgeClipBotOrigin, direction, raycastDistance, groundLayer);
            if (hitTop == false && hitBot is false)
            {
                // Set move speed (horizontal) directly, overrides last value
                rb2d.linearVelocityX = moveX * moveSpeed;
            }
            Debug.DrawLine(edgeClipTopOrigin, edgeClipTopOrigin + edgeClipRayDistance, hitTop ? Color.red : Color.green);
            Debug.DrawLine(edgeClipBotOrigin, edgeClipBotOrigin + edgeClipRayDistance, hitBot ? Color.red : Color.green);
        }
        // Synchronize the animator's parameters to this player's movement so it can
        // automatically control the player's animation.
        animator.SetFloat("moveSpeedX", Mathf.Abs(moveX));

        ///////////////////////////////////////////////////////////////////////////////
        /// JUMP

        // Additional gravity while falling
        if (rb2d.linearVelocityY < 0)
        {
            rb2d.AddForceY(fallGravity);
        }


        // Decrement coyote time timer
        coyoteTimeRemaining -= Time.deltaTime;

        // 
        Vector2 rayOrigin = this.transform.position;
        Vector2 rayDirection = Vector2.down;
        float distance = 1.05f;
        bool isGrounded = Physics2D.Raycast(rayOrigin, rayDirection, distance, groundLayer);
        if (isGrounded)
        {
            // Reset coyote time timer because we are on the ground
            coyoteTimeRemaining = maxCoyoteTime;
        }

        // Check if we can jump
        if (isGrounded == true || coyoteTimeRemaining > 0)
        {
            // Check if jump key pressed this frame
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Remove ability to coyote jump
                coyoteTimeRemaining = 0;
                // How much time player can continue jumping for
                jumpTimeRemaining = maxJumpTime;
            }
        }

        // If we can continue holding down jump
        if (jumpTimeRemaining > 0)
        {
            // Are we holding spacebar this frame?
            if (Input.GetKey(KeyCode.Space))
            {
                // Add force for jumping
                rb2d.linearVelocityY = jumpSpeed;
            }
            else
            {
                // End jump time
                jumpTimeRemaining = 0;
            }
            // Decrement timer
            jumpTimeRemaining -= Time.deltaTime;
        }

        animator.SetBool("isGrounded", isGrounded);
    }

    // Runs every time we change something in the inspector of this component,
    // or Reset is called, or when Unity recompiles, etc.
    private void OnValidate()
    {
        if (rb2d == null)
            rb2d = GetComponent<Rigidbody2D>();

        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

}
