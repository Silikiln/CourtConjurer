using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The base class for each ritual
/// </summary>
public abstract class Ritual : MonoBehaviour {
    protected static Ritual currentRitual;
    protected string creatureNotesWindowName = "targetCreatureNotes";
    /// <summary>
    /// Close the currently opened ritual, if any
    /// </summary>
    public static void CloseCurrentRitual() { if (currentRitual != null) currentRitual.CloseRitual(); }
    
    /// <summary>
    /// Determines if the ritual is ready to submit a component
    /// </summary>
    protected bool canSubmit = false;

    /// <summary>
    /// Called when the ritual is opened
    /// </summary>
    public virtual void ShowRitual()
    {
        currentRitual = this;
        GameManager.desk.SetActive(false);
        gameObject.SetActive(true);
        LoadCreatureNotes();
    }

    /// <summary>
    /// Called when the ritual is closing
    /// </summary>
    protected virtual void CloseRitual()
    {
        gameObject.SetActive(false);
        GameManager.desk.SetActive(true);
        currentRitual = null;
    }

    /// <summary>
    /// Called when the ritual is submitting its component
    /// </summary>
    protected virtual void OnSubmit()
    {
        canSubmit = false;
        Order.SubmittedComponents.Add(GetCurrentComponent());
    }

    /// <summary>
    /// Set the active state of all the children to the provided bool
    /// </summary>
    /// <param name="active">The state to set the children</param>
    protected void SetChildrenActive(bool active)
    {
        foreach (Transform t in transform)
            t.gameObject.SetActive(active);
    }

    /// <summary>
    /// Determines if the ritual is currently closing
    /// </summary>
    /// <returns>Whether or not the ritual is closing</returns>
    protected bool IsClosing()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseRitual();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines if the ritual is currently submitting a component
    /// </summary>
    /// <returns>Whether or not the ritual is submitting a component</returns>
    protected bool IsSubmitting()
    {
        if (canSubmit && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            OnSubmit();
            CloseRitual();
            return true;
        }

        return false;
    }

    protected void LoadCreatureNotes()
    {
        //check if there is already a prefab and destroy it
        if(currentRitual.gameObject.transform.FindChild(creatureNotesWindowName))
        {
            Destroy(currentRitual.gameObject.transform.FindChild(creatureNotesWindowName).gameObject);
        }

        //if there is a bookmarkedcreature and that creature has a ritual requirement that matches
        Creature tempCreature = GameManager.bookedCreature.getCreature();
        if (tempCreature != null)
        {
            //get the components of the creature and check which rituals are related?
            Component requiredComponent = tempCreature.RequiredComponents.FirstOrDefault(c => c.ComponentType == GetRitualType());
            if(requiredComponent != null)
            {
                string path = "Prefabs/Windows/" + creatureNotesWindowName;
                GameObject creatureNotesWindow = (GameObject)Instantiate(Resources.Load<GameObject>(path), new Vector3(-5, 2, 0), Quaternion.identity);
                creatureNotesWindow.name = creatureNotesWindowName;
                Debug.Log(creatureNotesWindow.name);
                Transform creatureNotesWindowTransform = creatureNotesWindow.transform;
                creatureNotesWindowTransform.parent = this.gameObject.transform;
            } 
        }
    }

    /// <summary>
    /// Gets the current component result of the ritual6
    /// </summary>
    /// <returns></returns>
    protected abstract Component GetCurrentComponent();
    protected abstract Component.Type GetRitualType();
}

