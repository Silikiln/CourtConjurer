using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Handles the logic for the rapid fire effigy ritual
/// </summary>
public class EffigyRitual : Ritual {
    public GameObject cutIndicator;

    private KeyCode[] inputsToCheck = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
    private float[] indicatorRotations = { 0, 90, 45, 135 };
    private byte[] cutsToMake;
    private List<byte> cutsMade = new List<byte>();

    void Start()
    {
        cutsToMake = new byte[0];
        ResetCuts();
    }

    void Update()
    {
        if (IsClosing())
            return;

        if (IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetCuts();

        for (int i = 0; i < inputsToCheck.Length; i++)
            if (Input.GetKeyDown(inputsToCheck[i]))
            {
                canSubmit = true;

                if (cutsToMake.Length > 0)
                {
                    if (i == cutsToMake[cutsMade.Count])
                        cutsMade.Add((byte)i);
                    else
                        cutsMade.Clear();

                    if (cutsMade.Count >= cutsToMake.Length)
                        Debug.Log("Correct Yo");
                    else
                        UpdateIndicator();
                }
                else cutsMade.Add((byte)i);

                string result = "";
                foreach (byte b in cutsMade) result += inputsToCheck[b].ToString();
                Debug.Log(result);
            }

        //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        //    foreach (Creature c in Creature.matchingCreatures(new EffigyRitual(cutsMade)))
        //        Debug.Log(c);

    }

    void ResetCuts()
    {
        cutsMade.Clear();
        canSubmit = false;
    }

    void UpdateIndicator()
    {
        cutIndicator.transform.rotation = Quaternion.Euler(0, 0, indicatorRotations[cutsToMake[cutsMade.Count]]);
    }

    protected override Component GetCurrentComponent()
    {
        return new Component(Component.Type.Effigy, cutsMade);
    }
}
