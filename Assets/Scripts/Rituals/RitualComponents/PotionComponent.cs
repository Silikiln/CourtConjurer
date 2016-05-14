using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PotionComponent : RitualComponent
{
    const float INGREDIENT_X_BETWEEN = 1;
    const float INGREDIENT_Y_BETWEEN = 1;

    public override Type ComponentType { get { return Type.Potion; } }

    public PotionComponent(List<RitualMaterial> ritualMaterials) : base(ritualMaterials) { }

    public override GameObject GetComponentVisual()
    {
        GameObject cauldron = base.GetComponentVisual();

        Vector3[] gridPositions = Grid.Generate(ritualMaterials.Count, INGREDIENT_X_BETWEEN, INGREDIENT_Y_BETWEEN, -1);
        for (int i = 0; i < ritualMaterials.Count; i++)
        {
            GameObject ingredient = ritualMaterials[i].GetMaterialResource<GameObject>();
            ingredient.transform.parent = cauldron.transform;
            ingredient.transform.localScale = new Vector3(1, 1, 1);
            ingredient.transform.localPosition = gridPositions[i];
        }

        return cauldron;
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
