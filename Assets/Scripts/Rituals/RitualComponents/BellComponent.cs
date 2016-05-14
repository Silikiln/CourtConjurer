using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BellComponent : RitualComponent
{
    const float BELL_Y_OFFSET = .15f;
    const float BELL_Y_BETWEEN = .9f;
    const float BELL_X_BETWEEN = 1.7f;
    Vector3 BELL_SCALE = new Vector3(.5f, .5f, 1);

    public override Type ComponentType { get { return Type.Bell; } }

    public byte[] BellPositions { get; private set; }

    public BellComponent(List<RitualMaterial> ritualMaterials, byte[] bellPositions) : base(ritualMaterials) {
        BellPositions = bellPositions;
    }

    public override GameObject GetComponentVisual()
    {
        GameObject musicStaff = base.GetComponentVisual();
        
        for (int i = 0; i < ritualMaterials.Count; i++)
        {
            GameObject bell = new GameObject();
            bell.transform.parent = musicStaff.transform;
            bell.transform.localScale = BELL_SCALE;
            bell.transform.localPosition = 
                new Vector3((BellPositions[i] - 2) * BELL_X_BETWEEN, BELL_Y_OFFSET - (i - 2) * BELL_Y_BETWEEN, -1);

            bell.AddComponent<SpriteRenderer>().sprite = ritualMaterials[i].GetMaterialResource<Sprite>();
        }

        return musicStaff;
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
