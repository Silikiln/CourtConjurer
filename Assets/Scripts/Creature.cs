using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;

/// <summary>
/// Data type to store information about a creature and how to summon it
/// </summary>
public class Creature
{
    public static List<Creature> loadedCreatures;
    public static List<string> uniqueAttributes;

    /// <summary>
    /// Load the creatures saved in an external file
    /// </summary>
    public static void LoadCreatures()
    {
        loadedCreatures = new List<Creature>();
        uniqueAttributes = new List<string>();

        using (XmlReader reader = XmlReader.Create("Assets/CreatureList.xml"))
        {
            List<RitualMaterial> ritualMaterials = new List<RitualMaterial>();
            List<byte> ritualData = new List<byte>();
            Creature currentCreature = new Creature();
            reader.ReadToFollowing("Creature");
            while (!reader.EOF)
            {
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    switch (reader.Name)
                    {
                        case "Creature":
                            currentCreature = new Creature();
                            break;
                        case "Title":
                            currentCreature.Title = reader.ReadInnerXml();
                            break;
                        case "Type":
                            currentCreature.Type = reader.ReadInnerXml();
                            break;
                        case "STR":
                            currentCreature.STR = byte.Parse(reader.ReadInnerXml());
                            break;
                        case "AGI":
                            currentCreature.AGI = byte.Parse(reader.ReadInnerXml());
                            break;
                        case "INT":
                            currentCreature.INT = byte.Parse(reader.ReadInnerXml());
                            break;
                        case "CHR":
                            currentCreature.CHR = byte.Parse(reader.ReadInnerXml());
                            break;

                        case "Attribute":
                            string attr = reader.ReadInnerXml();
                            currentCreature.Attributes.Add(attr);
                            if (!uniqueAttributes.Contains(attr))
                                uniqueAttributes.Add(attr);
                            break;
                        case "Name":
                            currentCreature.Names.Add(reader.ReadInnerXml());
                            break;

                        case "Totem":
                        case "Rune":
                        case "Potion":
                            ritualData = new List<byte>();
                            ritualMaterials = new List<RitualMaterial>();
                            break;

                        case "Material":
                            ritualMaterials.Add(RitualMaterial.Get(reader.ReadInnerXml()));
                            break;
                        case "Data":
                            ritualData.Add(byte.Parse(reader.ReadInnerXml()));
                            break;
                    }
                else
                    switch (reader.Name)
                    {
                        case "Creature":
                            loadedCreatures.Add(currentCreature);
                            break;
                        case "Totem":
                            currentCreature.RequiredComponents.Add(new TotemComponent(ritualMaterials));
                            break;
                        case "Rune":
                            currentCreature.RequiredComponents.Add(new RuneComponent(ritualMaterials, ritualData));
                            break;
                        case "Potion":
                            currentCreature.RequiredComponents.Add(new PotionComponent(ritualMaterials));
                            break;

                        case "Bells":                        
                        
                        
                        case "Incantation":
                            break;
                    }
            }
        }
    }

    /// <summary>
    /// Find all of the creatures that have a component
    /// </summary>
    /// <param name="toCheck">The component to check</param>
    /// <returns>A list of all of the creatures with that component</returns>
    public static List<Creature> CreaturesWithComponent(RitualComponent toCheck)
    {
        List<Creature> matchedCreatures = new List<Creature>();

        foreach (Creature c in loadedCreatures)
            foreach (RitualComponent otherComponent in c.RequiredComponents)
                // If the creature has the component, add it to the list
                // Move to the next creature
                if (toCheck.Equals(otherComponent))
                {
                    matchedCreatures.Add(c);
                    break;
                }

        return matchedCreatures;
    }

    /// <summary>
    /// Find all of the creatures that have all of the provided components
    /// </summary>
    /// <param name="componentsToCheck">The components to check</param>
    /// <returns>A list of all of the creatures with all of the components</returns>
    public static List<Creature> CreaturesWithComponents(List<RitualComponent> componentsToCheck)
    {
        if (componentsToCheck.Count == 0)
            return new List<Creature>();

        List<Creature> matchedCreatures = CreaturesWithComponent(componentsToCheck[0]);
        componentsToCheck.RemoveAt(0);

        foreach (RitualComponent component in componentsToCheck)
            matchedCreatures.Intersect(CreaturesWithComponent(component));

        return matchedCreatures;
    }

    /// <summary>
    /// The generic name of the creature
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// The type of the creature
    /// </summary>
    // TODO
    // Replace with enum and have int in XML file
    public string Type { get; private set; }

    /// <summary>
    /// The list of names from known creatures of this title.
    /// Mostly used with demons exclusively
    /// </summary>
    public List<string> Names = new List<string>();

    public byte STR { get; private set; }
    public byte AGI { get; private set; }
    public byte INT { get; private set; }
    public byte CHR { get; private set; }

    /// <summary>
    /// The list of attributes this creature has
    /// </summary>
    public List<string> Attributes = new List<string>();

    /// <summary>
    /// The components required to summon this creature
    /// </summary>
    public List<RitualComponent> RequiredComponents = new List<RitualComponent>();

    /// <summary>
    /// Determines if the provided components can satisfy
    /// the requirements for this creature
    /// </summary>
    /// <param name="componentsToCheck">The components to check</param>
    /// <returns>
    /// Whether this creature can be summoned using the
    /// provided components
    /// </returns>
    public bool fulfillsRequirements(List<RitualComponent> componentsToCheck)
    {
        if (RequiredComponents.Count > componentsToCheck.Count)
            return false;

        // Copy the required components
        List<RitualComponent> copiedComponents = new List<RitualComponent>(RequiredComponents);
        foreach (RitualComponent toCheck in componentsToCheck)
        {
            for (int i = 0; i < copiedComponents.Count; i++)
                if (toCheck.Matches(copiedComponents[i]))
                {
                    // Remove the matched required component from future checks
                    copiedComponents.RemoveAt(i);
                    break;
                }
        }

        // Determine if all required components matched 
        return copiedComponents.Count == 0;
    }

    public override string ToString()
    {
        return Title;
    }

    public Sprite FetchCreatureSprite()
    {
        string path = "Sprites/Creatures/" + this.Title.ToLower();
        Sprite creatureSprite = Resources.Load <Sprite>(path);

        if (creatureSprite == null)
            throw new MissingReferenceException("Could not load creature sprite for " + Title + " at path '" + path + "'");
            
        return creatureSprite;
    }

    public bool HasComponentOfType(RitualComponent.Type matchingType) { return RequiredComponents.Exists(c => c.ComponentType == matchingType); }

    public RitualComponent GetFirstComponentOfType(RitualComponent.Type matchingType)
    {
        return RequiredComponents.FirstOrDefault(c => c.ComponentType == matchingType);
    }
}
