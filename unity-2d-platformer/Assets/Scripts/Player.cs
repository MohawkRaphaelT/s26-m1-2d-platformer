using UnityEngine;

public class Player : MonoBehaviour
{
    // VARIABLES
    // We want to know about the player's RigidBody2D component to add forces to it
    public Rigidbody2D rb2d;
    // We want the player's animator component to synchronize its states to player movement
    public Animator animator;
    // We want to be able to control the sprite flipX to align with facing direction when we move
    public SpriteRenderer spriteRenderer;

    // How fast do we want the player to move?
    public float speedX = 1f;


    void Update()
    {
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

            // Set move speed (horizontal) directly, overrides last value
            rb2d.linearVelocityX = moveX * speedX;
        }
        // Synchronize the animator's parameters to this player's movement so it can
        // automatically control the player's animation.
        animator.SetFloat("moveSpeedX", Mathf.Abs(moveX));
    }

    // Runs every time we change something in the inspector of this component,
    // or Reset is called, or when Unity recompiles, etc.
    private void OnValidate()
    {
        if (rb2d == null)
            rb2d = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

}
