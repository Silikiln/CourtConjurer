using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Handles the logic for opening a specified ritual
/// </summary>
public class RitualOpen : MonoBehaviour {
    public GameObject ritualToOpen;
    public Sprite highlightSprite;
    public string sceneToLoad;

    private GameObject highlightObject;

    void OnMouseEnter()
    {
        highlightObject = new GameObject();
        highlightObject.transform.parent = transform;
        highlightObject.transform.position = this.transform.position;
        highlightObject.AddComponent<SpriteRenderer>().sprite = highlightSprite;
    }

    void OnMouseExit()
    {
        GameObject.Destroy(highlightObject);
    }

    void OnMouseUpAsButton()
    {
        loadNewScene();
        if (highlightObject != null) GameObject.Destroy(highlightObject);
    }

    public void loadNewScene()
    {
        if (!sceneToLoad.Equals(""))
        {
            //start the animation(couroutine?)
            //load scene as additive
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            GameManager.desk.SetActive(false);
        }
        else
            Debug.Log("No Specified Scene");
    }
}
