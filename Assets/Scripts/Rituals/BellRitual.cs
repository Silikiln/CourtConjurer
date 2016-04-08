using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Handles the logic for the rhythmic bell ritual
/// </summary>
public class BellRitual : Ritual
{
    public float TimePerPlaythrough = 4f; 

    public Vector3 meterStartPosition;
    public Vector3 meterEndPosition;

    public GameObject staffMeter;
    public GameObject[] bells;

    // TODO:
    // Replace with a single start value and distance between each point
    public GameObject[] xPositionMarkers;

    private float elapsedTime = 0;
    private int currentNote = 0;

    void Start()
    {
        BellSlide.xPositions = new float[xPositionMarkers.Length];
        for (int i = 0; i < xPositionMarkers.Length; i++)
            BellSlide.xPositions[i] = xPositionMarkers[i].transform.position.x;

        BellSlide.FarLeft = meterStartPosition.x;
        BellSlide.FarRight = meterEndPosition.x;

        canSubmit = true;
    }

    void Update()
    {
        if (IsClosing() || IsSubmitting())
            return;

        // When the meter has reached the end
        if ((elapsedTime += Time.deltaTime) >= TimePerPlaythrough)
        {
            currentNote = 0;
            elapsedTime = 0;
            foreach (GameObject b in bells) b.GetComponent<BellSlide>().ClearHighlight();
        }
            

        // Move meter across staff
        staffMeter.transform.localPosition = Vector3.Lerp(meterStartPosition, meterEndPosition, Mathf.SmoothStep(-.05f, 1.05f, elapsedTime / TimePerPlaythrough));
        if (currentNote < xPositionMarkers.Length && staffMeter.transform.position.x > xPositionMarkers[currentNote].transform.position.x)
        {
            foreach (GameObject b in bells) b.GetComponent<BellSlide>().PlaySound(currentNote);
            currentNote++;
        }
    }

    public override void ShowRitual()
    {
        base.ShowRitual();
        //BellSlide.ProperLocations = BookmarkedCreatureComponent();
    }

    public override RitualComponent.Type GetRitualType()
    {
        return RitualComponent.Type.Bell;
    }
}
