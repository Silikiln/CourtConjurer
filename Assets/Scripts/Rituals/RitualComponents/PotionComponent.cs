using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PotionComponent : RitualComponent
{
    public override Type ComponentType { get { return Type.Potion; } }

    public PotionComponent(List<RitualMaterial> ritualMaterials) : base(ritualMaterials) { }

    public override GameObject GetComponentVisual()
    {
        GameObject potion = base.GetComponentVisual();
        TextMesh text = potion.GetComponent<TextMesh>();
        foreach (RitualMaterial m in ritualMaterials)
            text.text += RitualMaterial.ByteToString(m.Category);

        return potion;
    }

    public override bool Matches(RitualComponent c)
    {
        if (!base.Matches(c)) return false;
        return ritualMaterials.SequenceEqual(c.RitualMaterials);
    }

    public override string ToString()
    {
        string result = "Potion:\n";
        foreach (RitualMaterial m in ritualMaterials)
            result += m.ToString() + "\n";
        return result;
    }
}
