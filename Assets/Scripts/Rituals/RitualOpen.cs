using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the logic for opening a specified ritual
/// </summary>
public class RitualOpen : MonoBehaviour {
    public GameObject ritualToOpen;
    public Sprite highlightSprite;

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
        ritualToOpen.GetComponent<Ritual>().ShowRitual();
        if (highlightObject != null) GameObject.Destroy(highlightObject);
    }
}
