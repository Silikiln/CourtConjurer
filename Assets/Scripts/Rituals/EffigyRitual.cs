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

    List<byte> totemTypeStack = new List<byte>();
    List<GameObject> totemStack = new List<GameObject>();
    int currentTotem = 0;
    
    void Start()
    {
        ResetTotem();
        highlightTotem.transform.localScale = totemStack[0].transform.localScale;
    }

    void Update()
    {
        if (IsClosing() || IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetTotem();

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                if (totemTypeStack[currentTotem] == 0)
                    totemTypeStack[currentTotem] = (byte)(totemSprites.Length - 1);
                else totemTypeStack[currentTotem]--;

            if (Input.GetKeyDown(KeyCode.RightArrow) && ++totemTypeStack[currentTotem] == totemSprites.Length)

                totemTypeStack[currentTotem] = 0;

            totemStack[currentTotem].GetComponent<SpriteRenderer>().sprite = totemSprites[totemTypeStack[currentTotem]];

            canSubmit = !totemTypeStack.Contains(0);
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentTotem == 0)
                {
                    if (totemTypeStack[currentTotem] > 0 && totemStack.Count < maxTotems)
                        AddTotem();
                }
                else if (totemTypeStack[currentTotem] == 0)
                    RemoveTotem(currentTotem--);
                else currentTotem--;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentTotem == totemStack.Count - 1 && totemTypeStack[currentTotem] > 0 && totemStack.Count < maxTotems)
                {
                    currentTotem++;
                    AddTotem();
                }
                else if (currentTotem < totemStack.Count - 1)
                    if (totemTypeStack[currentTotem] == 0)
                        RemoveTotem(currentTotem);
                    else currentTotem++;
            }

            highlightTotem.transform.position = totemStack[currentTotem].transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
            RemoveTotem(currentTotem);
    }

    void AddTotem()
    {
        GameObject newTotem = GameObject.Instantiate(totem);
        newTotem.GetComponent<SpriteRenderer>().sprite = totemSprites[0];
        newTotem.transform.parent = transform;

        totemStack.Insert(currentTotem, newTotem);
        totemTypeStack.Insert(currentTotem, 0);

        PositionTotems();
        canSubmit = false;
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

        PositionTotems();
    }

    void PositionTotems()
    {
        int index = 0;
        for (float yPos = heightDifference * (totemStack.Count / 2) - (totemStack.Count % 2 == 0 ? heightDifference / 2 : 0);
            index < totemStack.Count; index++, yPos -= heightDifference)
            totemStack[index].transform.position = new Vector3(0, yPos, index);

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

        AddTotem();
    }

    protected override Component GetCurrentComponent()
    {
        return new Component(Component.Type.Effigy, totemTypeStack);
    }
}
