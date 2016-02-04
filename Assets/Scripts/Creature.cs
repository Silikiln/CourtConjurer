using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Creature
{
    public static List<Creature> loadedCreatures;
    public static List<string> uniqueAttributes;

    public string Title { get; private set; }
    public string Type { get; private set; }

    public List<string> Names = new List<string>();
    public List<string> Attributes = new List<string>();
    public List<Component> RequiredComponents = new List<Component>();

    public bool fulfillsRequirements(List<Component> componentsToCheck)
    {
        if (RequiredComponents.Count > componentsToCheck.Count)
            return false;

        List<Component> copiedComponents = new List<Component>(componentsToCheck);
        foreach (Component required in RequiredComponents)
        {
            bool match = false;
            for (int i = 0; i < copiedComponents.Count; i++)
                if (copiedComponents[i].MatchesComponent(required, Names))
                {
                    copiedComponents.RemoveAt(i);
                    match = true;
                    break;
                }
            if (!match) return false;
        }

        return true;
    }

    public static List<Creature> matchingCreatures(Component toCheck)
    {
        List<Creature> matchedCreatures = new List<Creature>();

        foreach (Creature c in loadedCreatures)
            foreach (Component otherComponent in c.RequiredComponents)
                if (toCheck.MatchesComponent(otherComponent, c.Names))
                {
                    matchedCreatures.Add(c);
                    break;
                }

        return matchedCreatures;
    }

    public static List<Creature> matchingCreatures(List<Component> componentsToCheck)
    {
        if (componentsToCheck.Count == 0)
            return new List<Creature>();

        List<Creature> matchedCreatures = matchingCreatures(componentsToCheck[0]);
        componentsToCheck.RemoveAt(0);

        foreach (Component component in componentsToCheck)
            matchedCreatures.Intersect(matchingCreatures(component));

        return matchedCreatures;
    }

    public static void LoadCreatures()
    {
        loadedCreatures = new List<Creature>();
        uniqueAttributes = new List<string>();

        using (XmlReader reader = XmlReader.Create("Assets/CreatureList.xml"))
        {
            Component component = new Component(Component.Type.Bell);

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
                        case "Attribute":
                            string attr = reader.ReadInnerXml();
                            currentCreature.Attributes.Add(attr);
                            if (!uniqueAttributes.Contains(attr))
                                uniqueAttributes.Add(attr);
                            break;
                        case "Name":
                            currentCreature.Names.Add(reader.ReadInnerXml());
                            break;
                        case "Bells":
                            component = new Component(Component.Type.Bell);
                            break;
                        case "Bell":
                            component.addData(byte.Parse(reader.ReadInnerXml()));
                            break;
                        case "Effigy":
                            component = new Component(Component.Type.Effigy);
                            break;
                        case "Cut":
                            component.addData(byte.Parse(reader.ReadInnerXml()));
                            break;
                        case "Incantation":
                            currentCreature.RequiredComponents.Add(new Component(reader.ReadInnerXml().ToUpper()));
                            break;
                        case "Potion":
                            component = new Component(Component.Type.Potion);
                            break;
                        case "Material":
                            component.addData(byte.Parse(reader.ReadInnerXml()));
                            break;
                        case "Rune":
                            component = new Component(Component.Type.Rune);
                            break;
                        case "Point":
                            component.addData(byte.Parse(reader.ReadInnerXml()));
                            break;
                    }
                else
                    switch (reader.Name)
                    {
                        case "Creature":
                            loadedCreatures.Add(currentCreature);
                            break;
                        case "Bells":
                        case "Effigy":
                        case "Potion":
                        case "Rune":
                            currentCreature.RequiredComponents.Add(component);
                            break;
                    }
            }
        }
    }

    public override string ToString()
    {
        return Title;
    }
}
