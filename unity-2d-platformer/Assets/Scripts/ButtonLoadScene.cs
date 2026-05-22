using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour
{
    //public string sceneToLoad;

    //public void LoadScene()
    //{
    //    SceneManager.LoadScene(sceneToLoad);
    //}

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
