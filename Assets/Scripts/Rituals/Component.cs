using System.Collections.Generic;

public class Component
{
    public enum Type { Incantation, Rune, Bell, Effigy, Potion }

    public Type ComponentType { get; private set; }
    public string Incantation { get; private set; }

    // Rune - Connected points
    // Effigy - Cuts made
    // Potion - Ingredients added
    // Bell - Bells played
    private List<byte> data;

    public Component(string incantation)
    {
        ComponentType = Type.Incantation;
        Incantation = incantation;
    }

    public Component(Type componentType, List<byte> data)
    {
        this.ComponentType = componentType;
        this.data = data;
    }

    public Component(Type componentType) {
        this.ComponentType = componentType;
        data = new List<byte>();
    }

    public void addData(byte data) { this.data.Add(data); }

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
