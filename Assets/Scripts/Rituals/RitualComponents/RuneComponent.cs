using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RuneComponent : RitualComponent
{
    public override Type ComponentType { get { return Type.Rune; } }

    public byte[] ConnectedPoints { get; private set; }

    public RuneComponent(List<RitualMaterial> ritualMaterials, List<byte> connectedPoints) : base(ritualMaterials)
    {
        ConnectedPoints = connectedPoints.ToArray();
    }

    public RuneComponent(RitualMaterial runeMaterial, RitualMaterial dyeMaterial, List<byte> connectedPoints) 
        : base (new List<RitualMaterial>(new RitualMaterial[] { runeMaterial, dyeMaterial }))
    {
        ConnectedPoints = connectedPoints.ToArray();
    }

    public override GameObject GetComponentVisual()
    {
        GameObject runeParent = base.GetComponentVisual();

        GameObject rune = ritualMaterials[0].GetMaterialResource<GameObject>();
        rune.transform.parent = runeParent.transform;
        rune.transform.localScale = new Vector3(1, 1, 1);
        rune.transform.localPosition = new Vector3(0, 0, rune.transform.localPosition.z - 1);

        for (int i = 0; i < rune.transform.childCount - 1; i++)
            rune.transform.FindChild("Point" + i).GetComponent<Collider2D>().enabled = false;

        ImprovedLineRenderer lineRenderer = rune.GetComponent<ImprovedLineRenderer>();
        lineRenderer.SetColor(new LineColor.Solid(Color.black));

        foreach (byte b in ConnectedPoints)
        {
            lineRenderer.AddPoint(rune.transform.FindChild("Point" + b).transform.localPosition - new Vector3(0, 0, 1));
        }

        return runeParent;
    }

    public override bool Matches(RitualComponent c)
    {
        if (!base.Matches(c)) return false;
        Debug.Log(this);
        Debug.Log(c);
        RuneComponent other = c as RuneComponent;
        return ritualMaterials.SequenceEqual(other.RitualMaterials) && 
            (ConnectedPoints.SequenceEqual(other.ConnectedPoints) || ConnectedPoints.SequenceEqual(other.ConnectedPoints.Reverse()));
    }

    public override string ToString()
    {
        string result = "Rune:\n";
        foreach (RitualMaterial m in ritualMaterials)
            result += m.ToString() + "\n";
        foreach (byte b in ConnectedPoints)
            result += b + " ";
        return result;
    }
}
