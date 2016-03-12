using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Handles the logic for the rapid fire effigy ritual
/// </summary>
public class EffigyRitual : Ritual {
    public GameObject totem;
    public GameObject highlightTotem;
    public Sprite neutralHighlight, correctHighlight, incorrectHighlight;
    public Sprite[] totemSprites;

    public int maxTotems = 6;
    public float heightDifference = 1.1f;

    byte[] correctTotemTypes;
    Vector3 firstPosition;
    List<byte> totemTypeStack = new List<byte>();
    List<GameObject> totemStack = new List<GameObject>();
    int currentTotem = 0;
    
    void Start()
    {
        firstPosition = highlightTotem.transform.position;

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
                totemTypeStack[currentTotem] = (byte)(totemSprites.Length - 1);
            else if (Input.GetKeyDown(KeyCode.RightArrow) && ++totemTypeStack[currentTotem] == totemSprites.Length)
                totemTypeStack[currentTotem] = 0;

            HighlightTotem();
            totemStack[currentTotem].GetComponent<SpriteRenderer>().sprite = totemSprites[totemTypeStack[currentTotem]];
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
        correctTotemTypes = BookmarkedCreatureComponentData();
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
        if (totemStack.Count > 0)
            highlightTotem.transform.position = totemStack[currentTotem].transform.position;

        if (correctTotemTypes == null) return;
        if (currentTotem >= correctTotemTypes.Length || correctTotemTypes[currentTotem] != totemTypeStack[currentTotem])
            highlightTotem.GetComponent<SpriteRenderer>().sprite = incorrectHighlight;
        else highlightTotem.GetComponent<SpriteRenderer>().sprite = correctHighlight;
    }

    void AddTotem()
    {
        GameObject newTotem = GameObject.Instantiate(totem);
        newTotem.GetComponent<SpriteRenderer>().sprite = totemSprites[0];
        newTotem.transform.parent = transform;

        totemStack.Add(newTotem);
        int totemType = (int)(Random.value * 9999) % 6;
        totemTypeStack.Add((byte)totemType);
        newTotem.GetComponent<SpriteRenderer>().sprite = totemSprites[totemType];

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

    protected override Component GetCurrentComponent()
    {
        return new Component(Component.Type.Effigy, totemTypeStack);
    }
    public override Component.Type GetRitualType()
    {
        return Component.Type.Effigy;
    }
}
