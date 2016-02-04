using UnityEngine;
using System.Collections;

public class SelectCreature : MonoBehaviour {
    public Creature attachedCreature;

    void OnMouseUpAsButton()
    {
        Order.CompleteOrder(attachedCreature);
    }
}
