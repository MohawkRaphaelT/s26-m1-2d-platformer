using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public Transform[] waypoints;
    public int currentWaypointIndex;
    public float moveSpeed = 1;

    private List<Rigidbody2D> connectedObjects = new();

    private void Start()
    {
        Vector2 startPosition = waypoints[currentWaypointIndex].position;
        transform.position = startPosition;
    }

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
        // Move everything on platform
        foreach (Rigidbody2D connectedObject in connectedObjects)
        {
            Vector2 objectPosition = connectedObject.position + delta;
            connectedObject.MovePosition(objectPosition);
        }

        // Go to next waypoint if at waypoint
        bool isAtWaypoint = delta.magnitude < 0.01f;
        if (isAtWaypoint)
        {
            currentWaypointIndex++;
            currentWaypointIndex %= waypoints.Length;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D other = collision.rigidbody;
        if (other == null)
            return;

        if (connectedObjects.Contains(other) == false)
        {
            connectedObjects.Add(other);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D other = collision.rigidbody;
        if (other == null)
            return;

        if (connectedObjects.Contains(other) == true)
        {
            connectedObjects.Remove(other);
        }
    }

}
