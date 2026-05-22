using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public KeyCode ResetKey = KeyCode.R;
    
    void Update()
    {
        // If R key is pressed this frame
        if (Input.GetKeyDown(ResetKey) == true)
        {
            // Get current scene
            Scene currentScene = SceneManager.GetActiveScene();
            // Reset scene
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}
