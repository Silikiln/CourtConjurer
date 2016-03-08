using UnityEngine;
using System.Collections;

public class BellSlide : MonoBehaviour {
    public static float[] xPositions = { -5.97f, -4.27f, -2.57f, -0.87f, 0.83f, 2.53f, 4.23f, 5.93f };
    public static byte[] ProperLocations = { 1, 3, 2, 4, 2 };

    public byte bellIndex;
    public AudioClip bellSound;
    public Sprite correctHighlight, incorrectHighlight;

    private byte noteIndex = 0;
    private bool dragging = false;
    private SpriteRenderer highlightRenderer;

    void Start()
    {
        highlightRenderer = transform.FindChild("Highlight").GetComponent<SpriteRenderer>();
    }

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
            if (noteIndex != i && Distance(mouseX, xPositions[i]) < Distance(mouseX, xPositions[noteIndex]))
            {
                noteIndex = (byte)i;
                transform.position = new Vector3(xPositions[noteIndex], transform.position.y);
                ClearHighlight();
                break;
            }
    }

    float Distance(float a, float b) { return Mathf.Abs(a - b); }

    public byte GetIndex() { return noteIndex; }

    public void ClearHighlight() { highlightRenderer.enabled = false; }

    public void PlaySound(int targetIndex) {
        if (ProperLocations != null)
            if (ProperLocations[bellIndex] == targetIndex && noteIndex == targetIndex) {
                highlightRenderer.enabled = true;
                highlightRenderer.sprite = correctHighlight;
            } else if (targetIndex == noteIndex)
            {
                highlightRenderer.enabled = true;
                highlightRenderer.sprite = incorrectHighlight;
            }


        if (noteIndex == targetIndex)
            AudioSource.PlayClipAtPoint(bellSound, Camera.main.transform.position);
    }
}
