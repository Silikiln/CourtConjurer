using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RuneComponent : RitualComponent
{
    private const float DISTANCE_BETWEEN_PIECES = 3.7f;

    public override Type ComponentType { get { return Type.Rune; } }

    private byte[] connectedPoints;

    public RuneComponent(List<RitualMaterial> ritualMaterials, List<byte> connectedPoints) : base(ritualMaterials)
    {
        this.connectedPoints = connectedPoints.ToArray();
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

        foreach (byte b in connectedPoints)
        {
            lineRenderer.AddPoint(rune.transform.FindChild("Point" + b).transform.localPosition - new Vector3(0, 0, 1));
        }

        return runeParent;
    }

    public override bool Matches(RitualComponent c)
    {
        if (!base.Matches(c)) return false;

        RitualMaterial[] otherMaterials = c.RitualMaterials;
        if (ritualMaterials.Count != otherMaterials.Length) return false;

        for (int i = 0; i < ritualMaterials.Count; i++)
            if (!ritualMaterials[i].Equals(otherMaterials[i])) return false;

        return true;
    }

    public override string ToString()
    {
        string result = "Totem:\n";
        foreach (RitualMaterial m in ritualMaterials)
            result += m.ToString() + "\n";
        return result;
    }
}
