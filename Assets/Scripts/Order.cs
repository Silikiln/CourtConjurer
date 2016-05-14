using UnityEngine;
using System;
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
    public static List<RitualComponent> SubmittedComponents;

    /// <summary>
    /// Generate the next order the player needs to fulfill
    /// </summary>
    public static void GenerateOrder()
    {
        SubmittedComponents = new List<RitualComponent>();
        CurrentOrder = new Order();
        CurrentOrder.RequiredCreatures = new CreatureRequirements[] { CreatureRequirements.GenerateRequirements() };
    }

    /// <summary>
    /// Finish the order with an appropriate creature
    /// </summary>
    /// <param name="c">The creature that fulfills the order</param>
    public static void CompleteOrder(params Creature[] c)
    {
        if (c.Length == 0)
            throw new InvalidOperationException("An order cannot be completed with zero creatures");

        CurrentOrder.CompletedWith = c;
        GameManager.CompleteOrder(CurrentOrder);
    }

    /// <summary>
    /// The attribute needed to finish this order
    /// </summary>
    public CreatureRequirements[] RequiredCreatures { get; private set; }

    /// <summary>
    /// The creature that finished this order
    /// </summary>
    public Creature[] CompletedWith { get; private set; }

    public class CreatureRequirements
    {
        public int MinimumSTR { get; private set; }
        public int MinimumAGI { get; private set; }
        public int MinimumINT { get; private set; }
        public int MinimumCHR { get; private set; }

        public int MaximumSTR { get; private set; }
        public int MaximumAGI { get; private set; }
        public int MaximumINT { get; private set; }
        public int MaximumCHR { get; private set; }

        public bool CreatureMatches(Creature c)
        {
            if (!WithinRange(c.STR, MinimumSTR, MaximumSTR))
                return false;

            if (!WithinRange(c.AGI, MinimumAGI, MaximumAGI))
                return false;

            if (!WithinRange(c.INT, MinimumINT, MaximumINT))
                return false;

            if (!WithinRange(c.CHR, MinimumCHR, MaximumCHR))
                return false;

            return true;
        }

        private static bool WithinRange(int test, int min, int max)
        {
            return (test > min && (max == 0 || test <= max));
        }

        public static CreatureRequirements GenerateRequirements()
        {
            CreatureRequirements result = new CreatureRequirements();
            result.MinimumSTR = 3;

            return result;
        }
    }
}
