using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Order {
    public static Order CurrentOrder { get; private set; }
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
        GameManager.OrderCompleted(CurrentOrder);
        SubmittedComponents.Clear();
    }

    public string RequiredAttribute { get; private set; }
    public Creature CompletedWith { get; set; }

}
