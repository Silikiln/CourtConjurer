using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {
    public string typed;

	void OnMouseDown()
    {
        transform.parent.parent.GetComponent<IncantationRitual>().setCurrentScroll(gameObject);
        AddHighlight();
        GetComponent<ScrollPosition>().RemoveFromPosition();
    }

    void AddHighlight()
    {
        //add highlight to new currentScroll
        gameObject.transform.FindChild("HighLight").gameObject.SetActive(true);
    }

    public void RemoveHighlight()
    {
        //remove highlight from the currentScroll
        gameObject.transform.FindChild("HighLight").gameObject.SetActive(false);
    }
}
