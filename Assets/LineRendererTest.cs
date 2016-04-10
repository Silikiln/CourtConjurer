using UnityEngine;
using System.Collections;

public class LineRendererTest : MonoBehaviour {
    public Color lineColor;

    ImprovedLineRenderer lineRenderer;
    bool updated = false;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<ImprovedLineRenderer>();
        
	}

    void Update()
    {
        if (updated) return;
        lineRenderer.SetColor(new LineColor.Solid(lineColor));
        updated = true;
        lineRenderer.AddPoint(new Vector3(-1, -1));
        lineRenderer.AddPoint(new Vector3(1, 1));
    }
}
