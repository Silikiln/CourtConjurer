using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BellComponent : RitualComponent
{
    public override Type ComponentType { get { return Type.Bell; } }

    public byte[] BellPositions { get; private set; }

    public BellComponent(List<RitualMaterial> ritualMaterials, byte[] bellPositions) : base(ritualMaterials) {
        BellPositions = bellPositions;
    }

    public override GameObject GetComponentVisual()
    {
        GameObject bell = base.GetComponentVisual();
        TextMesh text = bell.GetComponent<TextMesh>();

        foreach (byte b in BellPositions)
            text.text += b + " ";
        text.text.Trim();

        return bell;
    }

    public override bool Matches(RitualComponent c)
    {
        if (!base.Matches(c)) return false;
        return RitualMaterials.SequenceEqual(c.RitualMaterials) && 
            BellPositions.SequenceEqual((c as BellComponent).BellPositions);
    }

    public override string ToString()
    {
        string result = "Bell:\n";
        for (int i = 0; i < ritualMaterials.Count; i++)
            result += ritualMaterials[i].ToString() + ": " + BellPositions[i] + "\n";
        return result;
    }
}
