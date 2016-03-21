using UnityEngine;
using System.Collections;
using System.Linq;

public class RitualNotes : MonoBehaviour {
    public static RitualNotes instance;

    TextMesh nameText, infoText;

	// Use this for initialization
	public void Initialize() {
        nameText = transform.FindChild("Title").GetComponent<TextMesh>();
        infoText = transform.FindChild("Data").GetComponent<TextMesh>();
	}

    public void TryLoad()
    {
        //if there is a bookmarkedcreature and that creature has a ritual requirement that matches
        if (BookmarkedPanel.BookmarkedCreature == null) return;

        Component requiredComponent = BookmarkedPanel.BookmarkedCreature.GetFirstComponentOfType(Ritual.CurrentRitual.GetRitualType());
        if (requiredComponent == null) return;

        nameText.text = BookmarkedPanel.BookmarkedCreature.Title;
        infoText.text = requiredComponent.GetContent();

        gameObject.SetActive(true);
    }
}
