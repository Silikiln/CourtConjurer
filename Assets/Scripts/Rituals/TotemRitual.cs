using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Handles the logic for the rapid fire effigy ritual
/// </summary>
public class TotemRitual : Ritual {
    public GameObject totem;
    public GameObject highlightTotem;
    public Sprite neutralHighlight, correctHighlight, incorrectHighlight;

    public int maxTotems = 6;
    public float heightDifference = 1.1f;

    RitualMaterial[] correctTotemTypes;
    Vector3 firstPosition;
    List<RitualMaterial> availableMaterials = new List<RitualMaterial>();
    List<byte> totemTypeStack = new List<byte>();
    List<GameObject> totemStack = new List<GameObject>();
    int currentTotem = 0;
    
    void Start()
    {
        firstPosition = highlightTotem.transform.position;

        availableMaterials.Add(RitualMaterial.Get("000"));
        availableMaterials.Add(RitualMaterial.Get("010"));

        AddTotem();
        highlightTotem.transform.localScale = totemStack[0].transform.localScale;
        ResetTotem();
    }

    void Update()
    {
        if (IsClosing() || IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetTotem();

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && --totemTypeStack[currentTotem] == 255)
                totemTypeStack[currentTotem] = (byte)(availableMaterials.Count - 1);
            else if (Input.GetKeyDown(KeyCode.RightArrow) && ++totemTypeStack[currentTotem] == availableMaterials.Count)
                totemTypeStack[currentTotem] = 0;

            HighlightTotem();
            totemStack[currentTotem].GetComponent<SpriteRenderer>().sprite = availableMaterials[totemTypeStack[currentTotem]].GetMaterialResource<Sprite>();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && currentTotem < totemStack.Count - 1)
            HighlightTotem(currentTotem + 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentTotem > 0)
            HighlightTotem(currentTotem - 1);
        else if (Input.GetKeyDown(KeyCode.Space) && totemStack.Count < maxTotems)
            AddTotem();
        else if (Input.GetKeyDown(KeyCode.Delete))
            RemoveTotem();
    }

    public override void ShowRitual()
    {
        base.ShowRitual();
        if (BookmarkedCreatureHasComponent())
            correctTotemTypes = BookmarkedCreatureComponent().RitualMaterials;
        else correctTotemTypes = null;
        highlightTotem.GetComponent<SpriteRenderer>().sprite = neutralHighlight;
        HighlightTotem();
    }

    void HighlightTotem(int index)
    {
        currentTotem = index;
        HighlightTotem();
    }

    void HighlightTotem() {
        highlightTotem.GetComponent<SpriteRenderer>().enabled = totemStack.Count > 0;
        if (totemStack.Count == 0) return;

        highlightTotem.transform.position = totemStack[currentTotem].transform.position;

        if (correctTotemTypes == null) return;
        if (currentTotem >= correctTotemTypes.Length || correctTotemTypes[currentTotem] != availableMaterials[totemTypeStack[currentTotem]])
            highlightTotem.GetComponent<SpriteRenderer>().sprite = incorrectHighlight;
        else highlightTotem.GetComponent<SpriteRenderer>().sprite = correctHighlight;
    }

    void AddTotem()
    {
        GameObject newTotem = GameObject.Instantiate(totem);
        newTotem.transform.parent = transform;

        totemStack.Add(newTotem);
        int totemType = Random.Range(0, availableMaterials.Count);
        totemTypeStack.Add((byte)totemType);
        newTotem.SetActive(true);
        newTotem.GetComponent<SpriteRenderer>().sprite = availableMaterials[totemType].GetMaterialResource<Sprite>();

        UpdateTotem();
    }

    void RemoveTotem()
    {
        if (totemStack.Count == 0)
            return;

        int index = totemStack.Count - 1;

        GameObject toRemove = totemStack[index];
        totemStack.RemoveAt(index);
        totemTypeStack.RemoveAt(index);

        GameObject.Destroy(toRemove);

        if (totemStack.Count > 0 && currentTotem == totemStack.Count)
            currentTotem--;

        UpdateTotem();
    }

    void UpdateTotem()
    {
        canSubmit = totemStack.Count > 0;

        for (int index = 0; index < totemStack.Count; index++)
            totemStack[index].transform.position = firstPosition + new Vector3(0, heightDifference * index, -index);
        HighlightTotem();
    }

    void ResetTotem()
    {
        currentTotem = 0;

        while (totemStack.Count > 0)
        {
            GameObject.Destroy(totemStack[totemStack.Count - 1]);
            totemStack.RemoveAt(totemStack.Count - 1);
        }
        
        totemTypeStack.Clear();
        UpdateTotem();
    }

    protected override RitualComponent GetCurrentComponent()
    {
        List<RitualMaterial> addedTotemPieces = new List<RitualMaterial>();
        foreach (byte b in totemTypeStack)
            addedTotemPieces.Add(availableMaterials[b]);
        return new TotemComponent(addedTotemPieces);
    }
    public override RitualComponent.Type GetRitualType()
    {
        return RitualComponent.Type.Totem;
    }
}
