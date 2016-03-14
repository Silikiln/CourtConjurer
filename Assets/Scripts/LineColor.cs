using UnityEngine;
using System.Collections.Generic;



public abstract class LineColor
{
    public abstract Color[] GetLineColors(List<Vector3> points);

    public class Solid : LineColor
    {
        private Color solidColor;
        public Solid(Color color) { solidColor = color; }

        public override Color[] GetLineColors(List<Vector3> points)
        {
            Color[] arr = new Color[(points.Count - 1) * 4];
            for (int i = 0; i < points.Count; i++) arr[i] = solidColor;
            return arr;
        }
    }

    public class Gradient : LineColor
    {
        private Color start, end;
        public Gradient(Color startColor, Color endColor)
        {
            start = startColor;
            end = endColor;
        }

        public override Color[] GetLineColors(List<Vector3> points)
        {
            float totalDistance = 0;
            float[] pointDistance = new float[points.Count];
            Vector3 currentPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                totalDistance += Vector3.Distance(currentPoint, points[i]);
                pointDistance[i] = totalDistance;
                currentPoint = points[i];
            }

            Color[] arr = new Color[(points.Count - 1) * 4];
            Color current = start;
            arr[0] = arr[1] = start;
            for (int i = 2; i < arr.Length - 2; i += 4)
            {
                Debug.Log(pointDistance[(i + 2) / 4] / totalDistance);
                for (int x = 0; x < 4; x++)
                    arr[i + x] = Color.Lerp(start, end, pointDistance[(i + 2) / 4] / totalDistance);
            }

            arr[arr.Length - 2] = arr[arr.Length - 1] = end;
            return arr;
        }
    }
}

