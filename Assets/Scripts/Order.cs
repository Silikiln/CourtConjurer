using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Order {
    public static List<Order> CompletedOrders = new List<Order>();
    public static Order CurrentOrder;
    public static List<Component> SubmittedComponents;

    public static void GenerateOrder()
    {
        SubmittedComponents = new List<Component>();
        CurrentOrder = new Order();
        CurrentOrder.RequiredAttribute = Creature.uniqueAttributes[Random.Range(0, Creature.uniqueAttributes.Count)];
    }

    public static void SubmitComponent(Component component)
    {
        SubmittedComponents.Add(component);
    }

    public static void CompleteOrder(Creature c)
    {
        CurrentOrder.CompletedWith = c;
        CompletedOrders.Add(CurrentOrder);
        SubmittedComponents.Clear();
        GenerateOrder();

        Debug.Log("Order complete (" + CompletedOrders.Count + ")");

        Ritual.CloseCurrentRitual();
    }

    public string RequiredAttribute { get; private set; }
    public Creature CompletedWith { get; set; }

}
