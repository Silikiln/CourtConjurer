using UnityEngine;
using System.Collections;

public class BellSlide : MonoBehaviour {
    public static float FarLeft, FarRight;
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
        noteIndex = 255;
        ClearHighlight();
    }

    void OnMouseUp()
    {
        dragging = false;
        noteIndex = 0;
        for (int i = 1; i < xPositions.Length; i++)
            if (NewPositionCloser(transform.position.x, xPositions[noteIndex], xPositions[i]))
                noteIndex = (byte)i;

        transform.position = new Vector3(xPositions[noteIndex], transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (!dragging) return;

        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        if (mouseX < FarLeft)
            mouseX = FarLeft;
        else if (mouseX > FarRight)
            mouseX = FarRight;

        transform.position = new Vector3(mouseX, transform.position.y, transform.position.z);

        
    }

    bool NewPositionCloser(float from, float originalPosition, float newPosition) {
        return Mathf.Abs(from - newPosition) < Mathf.Abs(from - originalPosition);
    }

    public byte GetIndex() { return noteIndex; }

    public void ClearHighlight() { highlightRenderer.enabled = false; }

    public void PlaySound(int targetIndex) {
        if (noteIndex == 255) return;

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
