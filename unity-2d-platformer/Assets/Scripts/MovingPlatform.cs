using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Debug")]
    public Color GizmosColor = Color.blue;
    public float GizmosScale = 1;

    [Header("Parameters")]
    public Rigidbody2D rb2d;
    public Transform[] waypoints;
    public int currentWaypointIndex;
    public float moveSpeed = 1;


    void FixedUpdate()
    {
        // Get current and next (target) positions
        Vector2 current = rb2d.transform.position;
        Vector2 next = waypoints[currentWaypointIndex].position;
        // Max distance to move towards targe
        float maxDistance = moveSpeed * Time.deltaTime;

        // Get new position moving in that direction without overshootign the target.
        Vector2 newPosition = Vector2.MoveTowards(current, next, maxDistance);

        // Delta means difference between 2 things.
        // Here it is the between previous and current position.
        Vector2 delta = newPosition - current;

        // Move platform
        rb2d.MovePosition(newPosition);

        // Go to next waypoint if at waypoint
        bool isAtWaypoint = delta.magnitude < 0.01f;
        if (isAtWaypoint)
        {
            // Increment by 1
            currentWaypointIndex++;
            // % is the remainder operator
            // If we are at the end of ther array, loop back to 0
            // eg. if current index is 5 and waypoints.length is 5, 
            // then 5 divided by 5 is 1 remainder 0
            currentWaypointIndex %= waypoints.Length;
        }
    }

    private void OnValidate()
    {
        // Stop running code here if the game is playing
        if (Application.isPlaying)
            return;

        // Only run if waypoints exists and has at least 1 waypoint
        if (waypoints.Length > 0 && waypoints[0] != null)
        {
            // Set start position when starting
            Vector2 startPosition = waypoints[currentWaypointIndex].position;
            transform.position = startPosition;
        }
    }

    private void OnDrawGizmos()
    {
        // Prevent running if we don't have waypoints
        if (waypoints == null || waypoints.Length == 0)
            return;

        // Set debug color
        Gizmos.color = GizmosColor;
        // Go through all waypoints minus last one
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            // Draw lines from-to waypoints
            Vector3 from = waypoints[i + 0].position;
            Vector3 to = waypoints[i + 1].position;
            Gizmos.DrawLine(from, to);
            // Draw box on first of two waypoints
            Gizmos.DrawCube(from, Vector3.one * GizmosScale);
        }
        // Draw box on last waypoint
        // [^1] array index means: 1 from the end of the array, or length - 1.
        Gizmos.DrawCube(waypoints[^1].position, Vector3.one * GizmosScale);
    }

}
