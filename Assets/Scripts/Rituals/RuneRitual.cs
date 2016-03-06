using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Handles the logic for the symbol drawing rune ritual
/// </summary>
public class RuneRitual : Ritual {
    List<byte> pointsConnected = new List<byte>();
    LineRenderer lineRenderer;
    Vector2 location;
    public Color good = Color.green;
    public Color neutral = Color.blue;
    public Color bad = Color.red;
    //int lineCount = 0;
    //int currentPosition = 0;
    float startPointDistance;
    float nextPointDistance;
    float tempDistance;
    Vector2[] pointVectors = new Vector2[9];
    public Vector2[] matchPattern = new Vector2[5];
    Double pointPosition;

    void Start()
    {
        canSubmit = true;
       
        //for (int x = -1; x <= 1; x++)
        //    pointVectors[2 + (x + 1) * 3] = new Vector2(x * GameObject.Find("Point" + i.ToString()).transform.position.x, GameObject.Find("Point" + i.ToString()).transform.position.y);
        //for (int x = -1; x <= 1; x++)
        //    pointVectors[1 + (x + 1) * 3] = new Vector2(x * 3, 0);
        //for (int x = -1; x <= 1; x++)
        //    pointVectors[0 + (x + 1) * 3] = new Vector2(x * 2, 2);

       
        startPointDistance = 9999;
        nextPointDistance = 9999;

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetWidth(0.1f, 0.1f);
        lineRenderer.SetColors(neutral, neutral);
    }

    void OnMouseOver()
    {
        location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            pointsConnected = new List<byte>();
            lineRenderer.SetVertexCount(1);
            lineRenderer.SetColors(neutral, neutral);
            //currentPosition = 0;
            startPointDistance = 9999;

            byte closest = 0;
            for (int i = 0; i < pointVectors.Length; i++)
            {
                tempDistance = Vector2.Distance(location, pointVectors[i]);

                if (tempDistance < startPointDistance)
                {
                    startPointDistance = tempDistance;
                    closest = (byte) i;
                }
            }

            pointsConnected.Add(closest);
            //lineRenderer.SetPosition(currentPosition, closestPoint);
            //currentPosition++;
        }

        if (Input.GetMouseButtonDown(1))
        {
            pointsConnected = new List<byte>();
            lineRenderer.SetColors(neutral, neutral);
            //currentPosition = 0;
        }
    }

    void Update()
    {
        int z = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                pointVectors[z] = new Vector2(GameObject.Find("Point" + z.ToString()).transform.position.x, GameObject.Find("Point" + z.ToString()).transform.position.y);
                z++;
            }
        }
        if (IsClosing())
            return;

        if (pointsConnected.Count > 0 && IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetPoints();

        nextPointDistance = 9999;

        if (Input.GetMouseButton(0))
        {
            for (int i = 0; i < pointVectors.Length; i++)
            {
                nextPointDistance = Vector2.Distance(location, pointVectors[i]);

                if (nextPointDistance <= 0.4 && !pointsConnected.Contains((byte)i))
                {
                    pointsConnected.Add((byte)i);
                }
            }
        }
        if (pointsConnected != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                lineRenderer.SetVertexCount(pointsConnected.Count);
                if (pointsConnected.Count > 0)
                    pointsConnected.RemoveAt(pointsConnected.Count - 1);
            }
            else
            {
                lineRenderer.SetVertexCount(pointsConnected.Count + 1);
            }
            for (int i = 0; i < pointsConnected.Count; i++)
            {
                lineRenderer.SetPosition(i, pointVectors[pointsConnected[i]]);
            }

            if (Input.GetMouseButton(0))
            {
                location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(pointsConnected.Count, location);
            }
        }
        else
        {
            lineRenderer.SetVertexCount(0);
        }
    }

    void ResetPoints()
    {
        pointsConnected.Clear();
        lineRenderer.SetVertexCount(0);
    }

    protected override Component GetCurrentComponent()
    {
        return new Component(Component.Type.Rune, pointsConnected);
    }
}
