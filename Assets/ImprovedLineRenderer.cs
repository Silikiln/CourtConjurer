using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ImprovedLineRenderer : MonoBehaviour {
    public float LineWeight = .3f;
    
    private List<Vector3> points = new List<Vector3>();
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        GetComponent<MeshFilter>().mesh = new Mesh();
        AddPoint(Vector3.zero);
        AddPoint(new Vector3(0, 2));
    }

    void UpdateMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        if (points.Count < 2) return;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    public void AddPoint(Vector3 point)
    {
        points.Add(point);

        if (points.Count == 1) return;

        Vector3 lastPoint = points[points.Count - 2];
        float angleBetween = Vector3.Angle(lastPoint, point);
        Debug.Log(lastPoint);
        Debug.Log(point);
        int vertexCount = vertices.Count;
        AddVertex(lastPoint, angleBetween + 90);
        AddVertex(lastPoint, angleBetween - 90);
        AddVertex(point, angleBetween + 90);
        AddVertex(point, angleBetween - 90);

        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount + 2);

        triangles.Add(vertexCount + 2);
        triangles.Add(vertexCount + 3);
        triangles.Add(vertexCount + 1);

        UpdateMesh();
    }

    void AddVertex(Vector3 point, float angleOffset)
    {
        angleOffset *= Mathf.Deg2Rad;
        vertices.Add(new Vector3(point.x + Mathf.Cos(angleOffset) * LineWeight, point.y + Mathf.Sin(angleOffset) * LineWeight, point.z));
        Debug.Log(string.Format("{0} [{1}] => {2}", point, angleOffset, vertices[vertices.Count - 1]));
    }

    [ContextMenu("Add Random Point")]
    void DoSomething()
    {
        AddPoint(new Vector3(Random.value * 2 - 1, Random.value * 2 - 1));        
    }
}
