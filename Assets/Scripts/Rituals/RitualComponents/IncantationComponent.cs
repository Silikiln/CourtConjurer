using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class IncantationComponent : RitualComponent
{
    public override Type ComponentType { get { return Type.Incantation; } }

    public string[] RequiredWords { get; private set; }

    public IncantationComponent(List<RitualMaterial> ritualMaterials, string words) : base(ritualMaterials) {
        RequiredWords = words.Split(' ');
    }

    public IncantationComponent(List<RitualMaterial> ritualMaterials, string[] words) : base(ritualMaterials)
    {
        RequiredWords = words;
    }

    public override GameObject GetComponentVisual()
    {
        GameObject incantation = base.GetComponentVisual();
        TextMesh text = incantation.GetComponent<TextMesh>();

        foreach (string s in RequiredWords)
            text.text += s + " ";
        text.text.Trim();

        return incantation;
    }

    public override bool Matches(RitualComponent c)
    {
        if (!base.Matches(c)) return false;

        return ritualMaterials.SequenceEqual(c.RitualMaterials) 
            && RequiredWords.SequenceEqual((c as IncantationComponent).RequiredWords);
    }

    public override string ToString()
    {
        string result = "Incantation:\n";
        for (int i = 0; i < ritualMaterials.Count; i++)
            result += ritualMaterials[i] + " - \"" + RequiredWords[i] + "\"\n";
        return result;
    }
}
