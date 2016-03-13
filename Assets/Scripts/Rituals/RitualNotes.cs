using UnityEngine;
using System.Collections;
using System.Linq;

public class RitualNotes : MonoBehaviour {
    public static RitualNotes instance;

    TextMesh nameText, infoText;

	// Use this for initialization
	void Start () {
        nameText = transform.FindChild("NoteHeader").GetComponent<TextMesh>();
        infoText = transform.FindChild("RitualInfo").GetComponent<TextMesh>();
	}

    public void TryLoad()
    {
        //if there is a bookmarkedcreature and that creature has a ritual requirement that matches
        if (BookmarkedPanel.BookmarkedCreature == null) return;

        Component requiredComponent = BookmarkedPanel.BookmarkedCreature.RequiredComponents.FirstOrDefault(c => c.ComponentType == Ritual.CurrentRitual.GetRitualType());
        if (requiredComponent == null) return;

        nameText.text = BookmarkedPanel.BookmarkedCreature.Title;
        infoText.text = requiredComponent.GetContent();

        gameObject.SetActive(true);
    }
}
