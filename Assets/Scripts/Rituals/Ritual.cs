using UnityEngine;
using System.Collections;

/// <summary>
/// The base class for each ritual
/// </summary>
public abstract class Ritual : MonoBehaviour {
    protected static Ritual currentRitual;

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

    /// <summary>
    /// Gets the current component result of the ritual
    /// </summary>
    /// <returns></returns>
    protected abstract Component GetCurrentComponent();
}
