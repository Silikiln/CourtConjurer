using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class RitualComponent
{
    public enum Type { Incantation, Rune, Bell, Totem, Potion, None }

    public abstract Type ComponentType { get; }

    public RitualComponent(List<RitualMaterial> ritualMaterials) { this.ritualMaterials = ritualMaterials; }

    protected List<RitualMaterial> ritualMaterials;

    public RitualMaterial[] RitualMaterials { get { return ritualMaterials.ToArray(); } }

    public virtual GameObject GetComponentVisual()
    {
        string path = "Prefabs/RitualComponents/" + ComponentType.ToString();
        GameObject result = GameObject.Instantiate(Resources.Load<GameObject>(path));

        if (result == null)
            throw new MissingReferenceException("Could not load component prefab for " + ComponentType.ToString());

        return result;
    }

    public virtual bool Matches(RitualComponent c)
    {
        if (ComponentType != c.ComponentType) return false;
        return true;
    }
}
