using UnityEngine;
using System.Collections;

public class ScrollPosition : MonoBehaviour {
    //locations of lock positions
    public static float[] xPositions;
    public static float[] yPositions;
    public static Scroll[] takenPositions;
    private byte scrollIndex = 255;
    void OnMouseUp() {
        //check position then lock or reset
        //compare x position of gameObject to marker positions to see which is the closest
        scrollIndex = 255;
        for (int i = 0; i < xPositions.Length; i++)
            if ((scrollIndex == 255|| NewPositionCloser(transform.position.x, xPositions[scrollIndex], xPositions[i])) && takenPositions[i] == null)
                scrollIndex = (byte)i;

        //set the transform equal to the noteIndex of the closest point
        transform.position = new Vector3(xPositions[scrollIndex], yPositions[scrollIndex], transform.position.z);
        takenPositions[scrollIndex] = GetComponent<Scroll>();
    }
    bool NewPositionCloser(float from, float originalPosition, float newPosition)
    {
        return Mathf.Abs(from - newPosition) < Mathf.Abs(from - originalPosition);
    }
    public void RemoveFromPosition()
    {
        if(scrollIndex != 255)
        {
            takenPositions[scrollIndex] = null;
        }
    }
}
