using UnityEngine;
using System.Collections;

public abstract class Ritual : MonoBehaviour {
    protected static Ritual currentRitual;

    public static void CloseCurrentRitual() { currentRitual.CloseRitual(); }

    protected bool canSubmit = false;

    public virtual void ShowRitual()
    {
        currentRitual = this;
        DeskObjectManager.SetDeskObjectsActive(false);
        gameObject.SetActive(true);
    }

    protected virtual void CloseRitual()
    {
        gameObject.SetActive(false);
        DeskObjectManager.SetDeskObjectsActive(true);
        currentRitual = null;
    }

    protected virtual void OnSubmit()
    {
        canSubmit = false;
        Order.SubmitComponent(GetCurrentComponent());
    }

    protected void SetChildrenActive(bool active)
    {
        foreach (Transform t in transform)
            t.gameObject.SetActive(active);
    }

    protected bool IsClosing()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseRitual();
            return true;
        }

        return false;
    }

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

    protected abstract Component GetCurrentComponent();
}
