using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public Transform[] waypoints;
    public int currentWaypointIndex;
    public float moveSpeed = 1;

    private List<Rigidbody2D> connectedObjects;


    void FixedUpdate()
    {
        Vector2 current = this.transform.position;
        Vector2 next = waypoints[currentWaypointIndex].position;
        float maxDistance = moveSpeed * Time.deltaTime;
        Vector2 newPosition = Vector2.MoveTowards(current, next, maxDistance);
        Vector2 delta = newPosition - current;
        // Move platform
        rb2d.MovePosition(newPosition);
        // Move everything on platform
        foreach (Rigidbody2D connectedObject in connectedObjects)
        {
            Vector2 objectPosition = connectedObject.position + delta;
            connectedObject.MovePosition(objectPosition);
        }

        bool isAtWaypoint = Vector2.Distance(newPosition, next) < 0.01f;
        if (isAtWaypoint)
        {
            currentWaypointIndex++;
            currentWaypointIndex %= waypoints.Length;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D other = collision.otherRigidbody;
        if (other == null)
            return;

        if (connectedObjects.Contains(other) == false)
        {
            connectedObjects.Add(other);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D other = collision.otherRigidbody;
        if (other == null)
            return;

        if (connectedObjects.Contains(other) == true)
        {
            connectedObjects.Remove(other);
        }
    }

}
