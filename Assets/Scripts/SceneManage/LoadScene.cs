using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScene : MonoBehaviour {
    public string sceneToLoad;

    public void loadNewScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
