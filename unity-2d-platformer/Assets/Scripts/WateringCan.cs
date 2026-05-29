using UnityEngine;

public class WateringCan : MonoBehaviour
{
    // Belongs to class
    public static int NumberCollected = 0;

    // Belongs to instance
    public bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collider2d)
    {
        // See if collider object is tagged as "Player"
        // (see Inspector tag on GameObject)
        if (collider2d.gameObject.CompareTag("Player") == true)
        {
            // Increment number of these collected
            NumberCollected += 1;
            Debug.Log($"Watering cans collected: {NumberCollected}");

            // Disable object on it's collected
            this.gameObject.SetActive(false);
            // This must be the last thing we do!
        }
    }
}
