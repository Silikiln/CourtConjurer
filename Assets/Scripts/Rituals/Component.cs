using System.Collections.Generic;

/// <summary>
/// Contains all the data to handle each type of possible component
/// </summary>
public class Component
{
    public enum Type { Incantation, Rune, Bell, Effigy, Potion, None}

    public Type ComponentType { get; private set; }
    public string Incantation { get; private set; }

    // Rune -   Connected points
    // Effigy - Cuts made
    // Potion - Ingredients added
    // Bell -   Bells played
    private List<byte> data;

    /// <summary>
    /// Create a new incantation component
    /// </summary>
    /// <param name="incantation">The actual incantation</param>
    public Component(string incantation)
    {
        ComponentType = Type.Incantation;
        Incantation = incantation;
    }

    /// <summary>
    /// Create a new component
    /// </summary>
    /// <param name="componentType">The type of component</param>
    /// <param name="data">The data part of the component</param>
    public Component(Type componentType, List<byte> data)
    {
        this.ComponentType = componentType;
        this.data = data;
    }

    /// <summary>
    /// Create a new component with no data
    /// </summary>
    /// <param name="componentType">The type of component</param>
    public Component(Type componentType) {
        this.ComponentType = componentType;
        data = new List<byte>();
    }

    public byte[] GetData() { return data.ToArray(); }

    /// <summary>
    /// Add data to the component
    /// </summary>
    /// <param name="data">The data to add</param>
    public void addData(byte data) { this.data.Add(data); }

    /// <summary>
    /// Determines if the provided component matches this one
    /// </summary>
    /// <param name="other">The other commponent</param>
    /// <param name="possibleNames">The list of possible creature names (for replacement)</param>
    /// <returns></returns>
    public bool MatchesComponent(Component other, List<string> possibleNames)
    {
        if (this.ComponentType != other.ComponentType)
            return false;

        if (this.ComponentType == Type.Incantation)
        {
            if (possibleNames.Count > 0)
            {
                foreach (string name in possibleNames)
                    if (other.Incantation.Replace("~", name.ToUpper()).Equals(Incantation))
                        return true;
            }
            else return this.Incantation == other.Incantation;
        } else {
            if (this.data.Count == other.data.Count)
            {
                for (int i = 0; i < data.Count; i++)
                    if (data[i] != other.data[i]) return false;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Converts the content of the component's data to a string
    /// </summary>
    public string GetContent()
    {
        string result = "";
        switch (ComponentType)
        {
            case Type.Bell:
                foreach (byte b in data) result += b + " ";
                return result;
            case Type.Incantation:
                return Incantation;
            case Type.Effigy:
                foreach (byte b in data) result += b + " ";
                return result;
            case Type.Rune:
                foreach (byte b in data) result += b + " ";
                return result;
            case Type.Potion:
                foreach (byte b in data) result += b + " ";
                return result;

            default: return "N/A";
        }
    }

    public override string ToString()
    {
        string result = base.ToString();
        switch (ComponentType)
        {
            case Type.Bell:
                result = "";
                foreach (byte b in data) result += b + " ";
                return string.Format("Bell: {0}", result);
            case Type.Incantation:
                return string.Format("Incantation: {0}", Incantation);
            default:
                return result;
        }
    }
}
