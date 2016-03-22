using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The data type that stores the required attribute for an order
/// and the creature that fulfills it
/// </summary>
public class Order {
    /// <summary>
    /// The current order the player is working towards
    /// </summary>
    public static Order CurrentOrder { get; private set; }

    /// <summary>
    /// The components the player has submitted for this order
    /// </summary>
    public static List<Component> SubmittedComponents;

    /// <summary>
    /// Generate the next order the player needs to fulfill
    /// </summary>
    public static void GenerateOrder()
    {
        SubmittedComponents = new List<Component>();
        CurrentOrder = new Order();
        CurrentOrder.RequiredAttribute = Creature.uniqueAttributes[Random.Range(0, Creature.uniqueAttributes.Count)];
    }

    /// <summary>
    /// Finish the order with an appropriate creature
    /// </summary>
    /// <param name="c">The creature that fulfills the order</param>
    public static void CompleteOrder(Creature c)
    {
        CurrentOrder.CompletedWith = c;
        GameManager.CompleteOrder(CurrentOrder);
    }

    /// <summary>
    /// The attribute needed to finish this order
    /// </summary>
    public string RequiredAttribute { get; private set; }

    /// <summary>
    /// The creature that finished this order
    /// </summary>
    public Creature CompletedWith { get; set; }

}
