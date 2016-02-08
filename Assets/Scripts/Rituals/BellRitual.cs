using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Handles the logic for the rhythmic bell ritual
/// </summary>
public class BellRitual : Ritual
{
    // Duration of the entire segment in seconds
    const float TIME_PER_SEGMENT = 6f;

    // Error bounds for the note from its intended beat
    const float ACCEPTABLE_NOTE_DISTANCE = .3f;

    // The Y value each possible note will be displayed at
    private float[] NOTE_Y_VALUES = { 1.5f, .5f, -.5f, -1.5f };

    private KeyCode[] INPUTS_TO_CHECK = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T };
    private string[] KEY_TEXT_VALUES = { "A", "B", "C", "D", "E" };

    public Vector3 meterStartPosition;
    public Vector3 meterEndPosition;
    public GameObject staffMeter;
    public GameObject musicStaff;
    public GameObject beat;
    public GameObject correctNote;
    public GameObject incorrectNote;

    private int meterNote = 0;
    private float elapsedTime;
    private float[] noteXValues;
    private byte[] notesPlayed;

    private int currentBeats = 1;

    public override void ShowRitual()
    {
        base.ShowRitual();
        elapsedTime = 0;
    }

    void Start()
    {
        GenerateStaff(PossibleBeats.Half);
        resetMeter();
    }

    void Update()
    {
        if (IsClosing() || IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            resetMeter();

        // Rotate to the next beats per segment
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (++currentBeats == 5)
                currentBeats = 1;
            GenerateStaff((PossibleBeats)((int)Mathf.Pow(2, currentBeats) + 1));
            resetMeter();
        }

        // When the segment has ended
        if (elapsedTime < TIME_PER_SEGMENT)
            if ((elapsedTime += Time.deltaTime) > TIME_PER_SEGMENT)
            {
                // If there is a missed note or no notes at all
                if (Array.Exists(notesPlayed, n => n == 9) || !Array.Exists(notesPlayed, n => n > 0))
                    resetMeter();
                else
                {
                    elapsedTime = TIME_PER_SEGMENT;
                    canSubmit = true;
                }
            }

        staffMeter.transform.localPosition = Vector3.Lerp(meterStartPosition, meterEndPosition, Mathf.SmoothStep(-.05f, 1.05f, elapsedTime / TIME_PER_SEGMENT));
        float meterX = staffMeter.transform.localPosition.x;
        if (meterNote < noteXValues.Length - 1 && noteXValues[meterNote + 1] - meterX < meterX - noteXValues[meterNote])
            meterNote++;

        if (notesPlayed[meterNote] == 0)
            for (int i = 0; i < INPUTS_TO_CHECK.Length && notesPlayed[meterNote] == 0; i++)
                if (Input.GetKeyDown(INPUTS_TO_CHECK[i]))
                {
                    bool validNote = Mathf.Abs(meterX - noteXValues[meterNote]) < ACCEPTABLE_NOTE_DISTANCE;
                    notesPlayed[meterNote] = (byte)(validNote ? i + 1 : 9);
                    GameObject currentNote = GameObject.Instantiate(validNote ? correctNote : incorrectNote);
                    currentNote.transform.parent = musicStaff.transform;
                    currentNote.transform.localPosition = new Vector3(meterX, NOTE_Y_VALUES[i]);
                    currentNote.transform.Find("Note Text").GetComponent<TextMesh>().text = KEY_TEXT_VALUES[i];
                }

    }

    /// <summary>
    /// Reset all notes and the current place of the meter
    /// </summary>
    private void resetMeter()
    {
        canSubmit = false;
        elapsedTime = 0;
        notesPlayed = new byte[notesPlayed.Length];
        foreach (Transform t in musicStaff.transform)
            if (t.tag == "Note")
                GameObject.Destroy(t.gameObject);
        meterNote = 0;
    }

    public enum PossibleBeats { Half = 3, Fourth = 5, Eigth = 9, Sixteenth = 17 }
    /// <summary>
    /// Clear the current staff and add the specified beats per segment
    /// </summary>
    /// <param name="beats">The beats to generate</param>
    public void GenerateStaff(PossibleBeats beats)
    {
        notesPlayed = new byte[(int)beats - 1];

        foreach (Transform t in musicStaff.transform)
            if (t.tag == "Beat")
                GameObject.Destroy(t.gameObject);

        noteXValues = new float[(int)beats - 1];
        for (int i = 1; i < (int)beats; i++)
        {
            GameObject currentBeat = GameObject.Instantiate(beat);
            currentBeat.transform.parent = musicStaff.transform;
            currentBeat.transform.localPosition = Vector3.Lerp(meterStartPosition, meterEndPosition, (float)i / (int)beats);
            noteXValues[i - 1] = currentBeat.transform.localPosition.x;
        }
    }

    protected override Component GetCurrentComponent()
    {
        return new Component(Component.Type.Bell, new List<byte>(notesPlayed));
    }
}
