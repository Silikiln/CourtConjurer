using UnityEngine;
using System.Collections;

public class ScrollPosition : MonoBehaviour {
    //locations of lock positions
    public static float[] xPositions;
    public static float[] yPositions;
    public static Scroll[] takenPositions;
    private byte noteIndex = 255;
    void OnMouseUp() {
        //check position then lock or reset
        //compare x position of gameObject to marker positions to see which is the closest
        noteIndex = 255;
        for (int i = 0; i < xPositions.Length; i++)
            if ((noteIndex == 255|| NewPositionCloser(transform.position.x, xPositions[noteIndex], xPositions[i])) && takenPositions[i] == null)
                noteIndex = (byte)i;

        //set the transform equal to the noteIndex of the closest point
        transform.position = new Vector3(xPositions[noteIndex], yPositions[noteIndex], transform.position.z);
        takenPositions[noteIndex] = GetComponent<Scroll>();
    }
    bool NewPositionCloser(float from, float originalPosition, float newPosition)
    {
        return Mathf.Abs(from - newPosition) < Mathf.Abs(from - originalPosition);
    }
    public void RemoveFromPosition()
    {
        if(noteIndex != 255)
        {
            takenPositions[noteIndex] = null;
        }
    }
}
