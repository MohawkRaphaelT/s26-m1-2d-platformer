using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Rigidbody2D target;
    public SpriteRenderer targetSpriteRenderer;
    public float lookAheadOffsetX;

    [Range(0f, 1f)]
    public float lerpValue = 0.5f;

    void FixedUpdate()
    {
        // See if we are facing left
        bool isFacingLeft = targetSpriteRenderer.flipX == true;

        // Ternary operator
        // this value = is this true ? this if true      : this if false   ;
        //float offsetX = isFacingLeft ? -lookAheadOffsetX : lookAheadOffsetX;

        float offsetX = isFacingLeft
            ? -lookAheadOffsetX // of true
            : +lookAheadOffsetX; // if false

        // We must maintain the camera's Z position and set only X and Y
        // Copy target X and Y
        Vector3 targetPosition = target.position;
        // Override target Z with camera's Z position
        targetPosition.z = this.transform.position.z;
        // Apply offset to camera X
        targetPosition.x += offsetX;

        // Set camera position to intermediate point between current position and target position
        Vector3 newPosition = Vector3.Lerp(this.transform.position, targetPosition, lerpValue);
        this.transform.position = newPosition;
    }
}
