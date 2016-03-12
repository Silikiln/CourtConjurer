using UnityEngine;
using System.Collections;

public class ListMover : MonoBehaviour {
    private bool dragging = false;
    public float FarLeft, FarRight, FarTop, FarBot;
    private Collider2D col;
    private Vector3 offset;

    void Start()
    {
        //probably move this to somewhere else so we can detect screen size dynamically
        var lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        var upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        FarLeft = lowerLeft.x;
        FarRight = upperRight.x;
        FarTop = upperRight.y;
        FarBot = lowerLeft.y;
        col = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        dragging = true;
        offset =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    void OnMouseUp()
    {
        dragging = false;
    }

    // Update is called once per frame
    void Update () {
        if (!dragging) return;

        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - offset.x;
        float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - offset.y;
        float colX = col.bounds.extents.x;
        float colY = col.bounds.extents.y;

        if (mouseX - colX < FarLeft)
            mouseX = FarLeft + colX;
        else if (mouseX + colX > FarRight)
            mouseX = FarRight - colX;

        if (mouseY - colY < FarBot)
            mouseY = FarBot + colY;
        else if (mouseY + colY > FarTop)
            mouseY = FarTop - colY;

        transform.position = new Vector3(mouseX, mouseY, transform.position.z);
    }
}
