using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// Handles the logic for the typing incantation ritual
/// </summary>
public class IncantationRitual : Ritual {
    private TextMesh typedMesh;
    public int scrollNumber;
    public int maxScriptLength;
    public GameObject scrollObject;
    private GameObject currentScroll;
    List<GameObject> scrolls = new List<GameObject>();
    public GameObject[] xPositionMarkers;

    private KeyCode[] alphabet = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
        KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Space, KeyCode.Quote
    };

    void Start()
    {
        Debug.Log("Starting");
        ScrollPosition.xPositions = new float[xPositionMarkers.Length];
        ScrollPosition.yPositions = new float[xPositionMarkers.Length];
        ScrollPosition.takenPositions = new Scroll[xPositionMarkers.Length];
        for (int i = 0; i < xPositionMarkers.Length; i++)
        {
            ScrollPosition.xPositions[i] = xPositionMarkers[i].transform.position.x;
            ScrollPosition.yPositions[i] = xPositionMarkers[i].transform.position.y;
            ScrollPosition.takenPositions[i] = null;
        }
    }

    public override void ShowRitual()
    {
        base.ShowRitual();
        GenerateScrolls();
        activateNextScroll();
    }

    void GenerateScrolls()
    {
        for (int i = 0; i < scrollNumber; i++)
        {
            scrolls.Add((GameObject)Instantiate(scrollObject));
            scrolls[i].transform.position += new Vector3(0, 0, i);
            scrolls[i].transform.parent = this.transform;
        }

        setCurrentScroll(scrolls[0]);
    }

    public void setCurrentScroll(GameObject tempScroll)
    {
        if(currentScroll != null)
        {
            currentScroll.GetComponent<Scroll>().RemoveHighlight();
        }
        //set currentScroll gameobject
        currentScroll = tempScroll;
        //sets the textMeshs equal to the current scroll's text fields
        typedMesh = currentScroll.transform.FindChild("Typed Text").GetComponent<TextMesh>();
    }

    void activateNextScroll()
    {
        //loop through the list of scrolls for the first one that is not active,
        //make that one active
        for (int i = 0; i < scrolls.Count; i++)
        {
            if (scrolls[i].activeSelf == false)
            {
                scrolls[i].SetActive(true);
                break;
            }
        }
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
                if (k == KeyCode.Space) currentScroll.GetComponent<Scroll>().typed += " ";
                else if (k == KeyCode.Quote) currentScroll.GetComponent<Scroll>().typed += "'";
                else currentScroll.GetComponent<Scroll>().typed += k.ToString();

                canSubmit = true;
                currentScroll.GetComponent<Scroll>().typed = RestrictString(currentScroll.GetComponent<Scroll>().typed);
                typedMesh.text = currentScroll.GetComponent<Scroll>().typed;
            }

    }
    public String RestrictString(String input)
    {
        if (input.Length <= maxScriptLength)
            return input;
        else
            return input.Substring(0, maxScriptLength);
    }

    void resetTypedText()
    {
        canSubmit = false;
        foreach (Scroll s in ScrollPosition.takenPositions)
            if (s.typed.Length > 0) { canSubmit = true; break; }

        currentScroll.GetComponent<Scroll>().typed = "";
        typedMesh.text = "";
    }
}
