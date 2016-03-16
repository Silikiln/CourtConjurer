using UnityEngine;
using System.Collections;

public class RunePoint : MonoBehaviour {
    public static RuneRitual parentScript;

    private int pointIndex;

    void Start()
    {
        pointIndex = transform.GetSiblingIndex();
        
    }

    void OnMouseUp()
    {
        parentScript.MouseButtonReleased();
    }

    void OnMouseEnter()
    {
        if (pointIndex == 4) Debug.Log("Test");
        parentScript.PointMousedOver(pointIndex, transform.localPosition - new Vector3(0, 0, 1));
    }

    void OnMouseDown()
    {
        parentScript.PointClicked(pointIndex, transform.localPosition - new Vector3(0, 0, 1));
    }
}
