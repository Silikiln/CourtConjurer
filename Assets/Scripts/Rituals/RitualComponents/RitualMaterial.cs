using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class RitualMaterial
{
    public byte Type { get; private set; }
    public byte Category { get; private set; }
    public byte Tier { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public RitualMaterial() { }
    public RitualMaterial(string input)
    {
        if (input.Length != 3)
            throw new ArgumentException(string.Format("A ritual material string must consist of three values. The provided value \"{0}\" has {1}", input, input.Length));

        Type = CharToByte(input[0]);
        Category = CharToByte(input[1]);
        Tier = CharToByte(input[2]);
    }
    public RitualMaterial(byte type, byte category, byte tier)
    {
        Type = type;
        Category = category;
        Tier = tier;
    }

    public override string ToString()
    {
        return string.Format("{0}{1}{2}", ByteToString(Type), ByteToString(Category), ByteToString(Tier));
    }


    public T GetMaterialResource<T>() where T : UnityEngine.Object
    {
        string path = "Materials/";
        switch (Type)
        {
            case 0:
                path += "Totem/";
                break;
            case 1:
                path += "Rune/";
                break;
            case 2:
                path += "Potion/";
                break;
            case 3:
                path += "Bell/";
                break;
            case 4:
                path += "Incantation/";
                break;
        }
        path += ByteToString(Category) + "" + ByteToString(Tier);
        T result = Resources.Load<T>(path);

        if (result == null)
            throw new MissingReferenceException("Could not load ritual material sprite for " + ToString() + " using path \"" + path + "\"");

        if (result is GameObject)
            result = GameObject.Instantiate(result);

        return result;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != GetType())
            return false;

        RitualMaterial other = obj as RitualMaterial;
        return Category == other.Category && Type == other.Type && (Tier == other.Tier || Tier == 255 || other.Tier == 255);
    }

    public override int GetHashCode()
    {
        return Tier * 35 + Category * 35 * 35 + Type * 35 * 35 * 35;
    }

    public static string ByteToString(byte b)
    {
        if (b < 10)
            return b.ToString();
        return ((char)('A' + b - 10)).ToString();
    }

    private static byte CharToByte(char c)
    {
        c = char.ToUpper(c);
        if (c == 'Z') return 255;
        byte b;
        if (byte.TryParse(c.ToString(), out b))
            return b;
        return (byte)(char.ToUpper(c) - 'A' + 10);
    }

    private static Dictionary<string, RitualMaterial> loadedMaterials;

    public static RitualMaterial Get(byte type, byte category, byte tier) {
        return Get(ByteToString(type) + ByteToString(category) + ByteToString(tier));
    }

    public static RitualMaterial Get(string key)
    {
        RitualMaterial result;
        if (!loadedMaterials.TryGetValue(key, out result))
            throw new ArgumentException("No ritual material has the ID of " + key);
        return result;
    }

    public static void LoadMaterials()
    {
        loadedMaterials = new Dictionary<string, RitualMaterial>();
        TextAsset materialXML = Resources.Load<TextAsset>("MaterialList");
        using (XmlTextReader reader = new XmlTextReader(new StringReader(materialXML.text)))
        {
            RitualMaterial currentMaterial = new RitualMaterial();
            reader.ReadToFollowing("Material");
            while (!reader.EOF)
            {
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    switch (reader.Name)
                    {
                        case "ID":
                            currentMaterial = new RitualMaterial(reader.ReadInnerXml()); ;
                            break;
                        case "Name":
                            currentMaterial.Name = reader.ReadInnerXml();
                            break;
                        case "Description":
                            currentMaterial.Description = reader.ReadInnerXml();
                            break;
                    }
                else if (reader.Name == "Material")
                    loadedMaterials.Add(currentMaterial.ToString(), currentMaterial);
            }
        }
    }
}
