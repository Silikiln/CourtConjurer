using UnityEngine;
using System.Collections;

/// <summary>
/// Select a creature who is ready to summon to finish an order
/// </summary>
public class SelectCreature : MonoBehaviour {
    public Creature attachedCreature;

    void OnMouseUpAsButton()
    {
        Order.CompleteOrder(attachedCreature);
    }
}
