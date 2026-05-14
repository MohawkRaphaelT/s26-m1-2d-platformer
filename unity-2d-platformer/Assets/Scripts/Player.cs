using UnityEngine;

public class Player : MonoBehaviour
{
    // VARIABLES
    // We want to know about the player's RigidBody2D component to add forces to it
    public Rigidbody2D rb2d;
    // We want the player's animator component to synchronize its states to player movement
    public Animator animator;
    // How fast do we want the player to move?
    public float speedX = 1f;
    //
    public CapsuleCollider2D capsuleCollider;
    public LayerMask groundLayerMask;

    //
    public float jumpForce = 5;
    public float jumpTimeMax = 0.3f;
    public float fallMultiplier = 2.5f;



    // debug
    private Vector2 GroundOrigin;
    private Vector2 GroundTarget;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeRemaining;

    public float GroundCheckDistance = 0.1f;


    void Start()
    {
        // Easy way to get the reference to the two components
        // GetComponent asks this GameObject for the variable type.
        // It returns the first one it finds, null if not attached.
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        // Get the player's movement input from Unity's legacy input system
        float moveX = Input.GetAxis("Horizontal");
        // Math.Abs() gives us the number's absolute value
        // eg. Abs(+1) and Abs(-1) both give us +1.
        if (Mathf.Abs(moveX) > 0.1f)
        {
            // Calculate the force to apply to the player (in Newtons if you want to look that up).
            rb2d.linearVelocityX = moveX * speedX;
        }
        // Synchronize the animator's parameters to this player's movement so it can
        // automatically control the player's animation.
        animator.SetFloat("moveSpeedX", Mathf.Abs(moveX));

        GroundOrigin = new Vector2(transform.position.x, transform.position.y - capsuleCollider.bounds.extents.y);
        GroundTarget = GroundOrigin + Vector2.down * GroundCheckDistance;
        isGrounded = Physics2D.Raycast(GroundOrigin, Vector2.down, GroundCheckDistance, groundLayerMask);
        
        // On ground, we can jump
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                jumpTimeRemaining = jumpTimeMax;
            }
        }

        // In air
        if (isGrounded is false)
        {
            jumpTimeRemaining -= Time.deltaTime;
            bool timeStopJump = jumpTimeRemaining <= 0;
            bool userStopJump = !Input.GetKey(KeyCode.Space);
            isJumping = !(timeStopJump || userStopJump);
        }

        // if jumping, maintain jump force
        if (isJumping is true)
        {
            rb2d.linearVelocityY = jumpForce;
        }

        // falling
        if (rb2d.linearVelocityY < 0 || isJumping is false)
        {
            rb2d.linearVelocityY += Physics2D.gravity.y * Time.deltaTime * (fallMultiplier - 1f);
        }

        //
        animator.SetBool("isGrounded", isGrounded);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(GroundOrigin, GroundTarget);
    }

    private void OnValidate()
    {
        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
}
