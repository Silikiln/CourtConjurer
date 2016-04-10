using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TotemComponent : RitualComponent
{
    private const float DISTANCE_BETWEEN_PIECES = 3.7f;

    public override Type ComponentType { get { return Type.Totem; } }

    public TotemComponent(List<RitualMaterial> ritualMaterials) : base(ritualMaterials) { }

    public override GameObject GetComponentVisual()
    {
        GameObject totem = base.GetComponentVisual();
        float startY = -DISTANCE_BETWEEN_PIECES * (ritualMaterials.Count / 2) + (ritualMaterials.Count % 2 == 0 ? DISTANCE_BETWEEN_PIECES / 2 : 0);
        for (int i = 0; i < ritualMaterials.Count; i++)
        {
            GameObject piece = new GameObject();
            piece.transform.parent = totem.transform;
            piece.transform.localPosition = new Vector3(0, startY + DISTANCE_BETWEEN_PIECES * i, -i - 1);
            piece.transform.localScale = new Vector3(1, 1, 1);

            SpriteRenderer renderer = piece.AddComponent<SpriteRenderer>();
            renderer.sprite = ritualMaterials[i].GetMaterialResource<Sprite>();
        }

        return totem;
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
