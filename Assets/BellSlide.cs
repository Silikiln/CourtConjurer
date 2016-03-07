using UnityEngine;
using System.Collections;

public class BellSlide : MonoBehaviour {
    public static float[] xPositions = { -5.97f, -4.27f, -2.57f, -0.87f, 0.83f, 2.53f, 4.23f, 5.93f };

    public AudioClip bellSound;

    private byte index = 0;
    private bool dragging = false;

	// Use this for initialization
	void OnMouseDown () {
        dragging = true;
	}

    void OnMouseUp()
    {
        dragging = false;
    }

    void Update()
    {
        if (!dragging) return;

        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        for (int i = 0; i < xPositions.Length; i++)
            if (index != i && Distance(mouseX, xPositions[i]) < Distance(mouseX, xPositions[index]))
            {
                index = (byte)i;
                transform.position = new Vector3(xPositions[index], transform.position.y);
                break;
            }
    }

    float Distance(float a, float b) { return Mathf.Abs(a - b); }

    public byte GetIndex() { return index; }

    public bool PlaySound(int targetIndex) {
        if (index != targetIndex) return false;
            
        AudioSource.PlayClipAtPoint(bellSound, Camera.main.transform.position);
        return true;
    }
}
