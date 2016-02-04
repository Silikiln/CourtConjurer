using UnityEngine;
using System.Collections;

public class DeskObjectManager : MonoBehaviour
{
    public static GameObject desk;

    void Start() {
        Creature.LoadCreatures();
        Order.GenerateOrder();
        desk = GameObject.Find("Desk");
    }

    public static void SetDeskObjectsActive(bool active)
    {
        foreach (Transform t in desk.transform)
            t.gameObject.SetActive(active);
    }
}
