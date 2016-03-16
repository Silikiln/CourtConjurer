using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Handles the logic for the symbol drawing rune ritual
/// </summary>
public class RuneRitual : Ritual {
    public Color good = Color.green, neutral = Color.blue, bad = Color.red;

    List<byte> pointsConnected = new List<byte>();
    ImprovedLineRenderer lineRenderer;
    bool drawing;

    byte[] properPoints;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<ImprovedLineRenderer>();
        //lineRenderer.SetColor(new LineColor.Solid(neutral));
        drawing = false;
        RunePoint.parentScript = this;
    }

    Vector3 MousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 9);
    }

    // When a point has been clicked
    public void PointClicked(int index, Vector3 pointPosition)
    {
        drawing = true;

        if (!pointsConnected.Contains((byte) index))
        {
            pointsConnected.Add((byte)index);
            lineRenderer.AddPoint(pointPosition);
        } else
        {
            int connectedIndex = pointsConnected.IndexOf((byte)index);
            while (pointsConnected.Count > connectedIndex + 1)
            {
                pointsConnected.RemoveAt(pointsConnected.Count - 1);
                lineRenderer.RemovePoint(lineRenderer.Count - 1);
            }
        }

        lineRenderer.AddPoint(MousePosition());
    }

    // When a point has been moused over
    public void PointMousedOver(int index, Vector3 pointPosition)
    {
        if (!drawing) return;

        if (pointsConnected.Count > 1 && pointsConnected[pointsConnected.Count - 1] == index)
        {
            pointsConnected.RemoveAt(pointsConnected.Count - 1);
            lineRenderer.RemovePoint(lineRenderer.Count - 1);
            lineRenderer.SetPoint(lineRenderer.Count - 1, MousePosition());
        } else if (!pointsConnected.Contains((byte) index))
        {
            pointsConnected.Add((byte)index);
            lineRenderer.SetPoint(lineRenderer.Count - 1, pointPosition);
            lineRenderer.AddPoint(MousePosition());
        }

        canSubmit = pointsConnected.Count > 1;
    }

    public void MouseButtonReleased()
    {
        drawing = false;
        lineRenderer.RemovePoint(lineRenderer.Count - 1);
    }

    void Update()
    {
        if (IsClosing() || IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetPoints();

        if (!drawing) return;

        lineRenderer.SetPoint(lineRenderer.Count - 1, MousePosition());
    }

    void ResetPoints()
    {
        pointsConnected.Clear();
        lineRenderer.Clear();
        canSubmit = false;
    }

    protected override Component GetCurrentComponent()
    {
        return new Component(Component.Type.Rune, pointsConnected);
    }
    public override Component.Type GetRitualType()
    {
        return Component.Type.Rune;
    }
}
