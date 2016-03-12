using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The base class for each ritual
/// </summary>
public abstract class Ritual : MonoBehaviour {
    public static Ritual CurrentRitual { get; protected set; }
    protected string creatureNotesWindowName = "targetCreatureNotes";
    /// <summary>
    /// Close the currently opened ritual, if any
    /// </summary>
    public static void CloseCurrentRitual() { if (CurrentRitual != null) CurrentRitual.CloseRitual(); }
    
    /// <summary>
    /// Determines if the ritual is ready to submit a component
    /// </summary>
    protected bool canSubmit = false;

    /// <summary>
    /// Called when the ritual is opened
    /// </summary>
    public virtual void ShowRitual()
    {
        CurrentRitual = this;
        GameManager.desk.SetActive(false);
        gameObject.SetActive(true);
        RitualNotes.instance.TryLoad();
    }

    /// <summary>
    /// Called when the ritual is closing
    /// </summary>
    protected virtual void CloseRitual()
    {
        RitualNotes.instance.gameObject.SetActive(false);
        gameObject.SetActive(false);
        GameManager.desk.SetActive(true);
        CurrentRitual = null;
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

    protected byte[] BookmarkedCreatureComponentData()
    {
        return BookmarkedPanel.BookmarkedCreature != null &&
            BookmarkedPanel.BookmarkedCreature.HasComponentOfType(GetRitualType()) ?
            BookmarkedPanel.BookmarkedCreature.GetFirstComponentOfType(GetRitualType()).GetData() : null;
    }

    protected bool BookmarkedCreatureHasComponent()
    {
        return BookmarkedPanel.BookmarkedCreature != null 
            && BookmarkedPanel.BookmarkedCreature.GetFirstComponentOfType(GetRitualType()) != null;
    }

    /// <summary>
    /// Gets the current component result of the ritual6
    /// </summary>
    /// <returns></returns>
    protected abstract Component GetCurrentComponent();
    public virtual Component.Type GetRitualType() { return Component.Type.None; }
}

