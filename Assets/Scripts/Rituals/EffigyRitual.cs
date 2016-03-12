using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Handles the logic for the rapid fire effigy ritual
/// </summary>
public class EffigyRitual : Ritual {
    public GameObject totem;
    public GameObject highlightTotem;
    public Sprite[] totemSprites;

    public int maxTotems = 6;
    public float heightDifference = 1.1f;

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

            totemStack[currentTotem].GetComponent<SpriteRenderer>().sprite = totemSprites[totemTypeStack[currentTotem]];
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && currentTotem < totemStack.Count - 1)
            highlightTotem.transform.position = totemStack[++currentTotem].transform.position;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentTotem > 0)
            highlightTotem.transform.position = totemStack[--currentTotem].transform.position;
        else if (Input.GetKeyDown(KeyCode.Space) && totemStack.Count < maxTotems)
            AddTotem();
        else if (Input.GetKeyDown(KeyCode.Delete))
            RemoveTotem(currentTotem);
    }

    void AddTotem()
    {
        GameObject newTotem = GameObject.Instantiate(totem);
        newTotem.GetComponent<SpriteRenderer>().sprite = totemSprites[0];
        newTotem.transform.parent = transform;
        newTotem.transform.position -= new Vector3(0, 0, totemStack.Count);

        totemStack.Add(newTotem);
        totemTypeStack.Add(0);

        UpdateTotem();
    }

    void RemoveTotem(int index)
    {
        if (totemStack.Count == 1)
            return;

        if (currentTotem == totemStack.Count - 1)
            currentTotem--;

        GameObject toRemove = totemStack[index];
        totemStack.RemoveAt(index);
        totemTypeStack.RemoveAt(index);

        GameObject.Destroy(toRemove);

        UpdateTotem();
    }

    void UpdateTotem()
    {
        highlightTotem.GetComponent<SpriteRenderer>().enabled = totemStack.Count > 0;
        canSubmit = totemStack.Count > 0;

        for (int index = 0; index < totemStack.Count; index++)
            totemStack[index].transform.position = firstPosition + new Vector3(0, heightDifference * index, -index);

        if (totemStack.Count > 0)
            highlightTotem.transform.position = totemStack[currentTotem].transform.position;
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
    protected override Component.Type GetRitualType()
    {
        return Component.Type.Effigy;
    }
}
