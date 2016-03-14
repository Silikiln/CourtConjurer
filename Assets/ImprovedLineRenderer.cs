using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ImprovedLineRenderer : MonoBehaviour {
    public float LineWeight = .3f;

    private bool _delayMeshUpdate = false;
    public bool DelayMeshUpdate {
        get { return _delayMeshUpdate; }
        set
        {
            _delayMeshUpdate = value;
            if (!_delayMeshUpdate)
                UpdateMesh();
        }
    }
    public LineColor colorHandler;

    private const float PIOver2 = Mathf.PI / 2;
    
    private List<Vector3> points = new List<Vector3>();
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        colorHandler = new LineColor.Gradient(Color.red, Color.cyan);

        GetComponent<MeshFilter>().mesh = new Mesh();
        AddPoint(Vector3.zero);
        AddPoint(new Vector3(2, 0));
    }

    void UpdateMesh()
    {
        if (DelayMeshUpdate) return;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        if (points.Count < 2) return;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        UpdateColors();
            
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    void UpdateColors()
    {
        if (colorHandler == null) return;
        GetComponent<MeshFilter>().mesh.colors = colorHandler.GetLineColors(points);
    }

    public void AddPoint(Vector3 point)
    {
        points.Add(point);
        if (points.Count == 1) return;
        int i;
        for (i = 0; i < 4; i++) vertices.Add(Vector3.zero);

        i = (points.Count - 2) * 4;
        triangles.AddRange(new int[] {
            i, i + 1, i + 2,
            i + 2, i + 3, i + 1
        });

        GenerateVertices(points.Count - 1);
    }

    public void SetPoint(int index, Vector3 point)
    {
        points[index] = point;
        GenerateVertices(index);
    }

    void GenerateVertices(int pointIndex)
    {
        if (pointIndex > 0)
            GenerateVertices(pointIndex - 1, pointIndex);
        if (pointIndex < points.Count - 1)
            GenerateVertices(pointIndex, pointIndex + 1);
        UpdateMesh();
    }

    void GenerateVertices(int aIndex, int bIndex)
    {
        Vector3 a = points[aIndex], b = points[bIndex];
        float angleBetween = AngleBetween(a, b);
        int vertexCount = vertices.Count;
        SetVertex(a, aIndex * 4, angleBetween);
        SetVertex(b, aIndex * 4 + 2, angleBetween);
    }

    float AngleBetween(Vector3 from, Vector3 to)
    {
        Vector3 result = from - to;
        float angle = Mathf.Atan2(result.y, result.x);
        return angle;
    }

    void SetVertex(Vector3 point, int vertexIndex, float angleOffset)
    {
        angleOffset += PIOver2;
        vertices[vertexIndex] = new Vector3(point.x + Mathf.Cos(angleOffset) * LineWeight, point.y + Mathf.Sin(angleOffset) * LineWeight, point.z);
        angleOffset -= Mathf.PI;
        vertices[vertexIndex + 1] = new Vector3(point.x + Mathf.Cos(angleOffset) * LineWeight, point.y + Mathf.Sin(angleOffset) * LineWeight, point.z);

    }

    [ContextMenu("Add Random Point")]
    void DoSomething()
    {
        AddPoint(new Vector3(Random.value * 20 - 10, Random.value * 20 - 10));        
    }
}
