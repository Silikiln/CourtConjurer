using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Handles the logic for the typing incantation ritual
/// </summary>
public class IncantationRitual : Ritual {
    public GameObject toTypeMesh;
    public GameObject typedMesh;
    public string toType;
    private string typed = "";

    private KeyCode[] alphabet = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
        KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Space, KeyCode.Quote
    };

    public override void ShowRitual()
    {
        base.ShowRitual();
        toTypeMesh.GetComponent<TextMesh>().text = toType;
        resetTypedText();
    }

    void Update()
    {
        if (IsClosing())
            return;

        if (IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            resetTypedText();

        foreach (KeyCode k in alphabet)
            if (Input.GetKeyDown(k))
            {
                if (k == KeyCode.Space) typed += " ";
                else if (k == KeyCode.Quote) typed += "'";
                else typed += k.ToString();

                if (toType.Length > 0)
                {
                    if (typed.Length < toType.Length)
                    {
                        if (toType[typed.Length] == '\n')
                            typed += '\n';
                        else if (toType[typed.Length] == '\r')
                            typed += "\r\n";
                    }
                }
                canSubmit = true;
                typedMesh.GetComponent<TextMesh>().text = typed;
            }

        //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        //    foreach (Creature c in Creature.matchingCreatures(typed.ToUpper()))
        //        Debug.Log(c);

    }

    void resetTypedText()
    {
        canSubmit = false;
        typed = "";
        typedMesh.GetComponent<TextMesh>().text = "";
    }

    protected override Component GetCurrentComponent()
    {
        return new Component(typed);
    }
}
