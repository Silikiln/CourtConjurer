using UnityEngine;
using System.Collections;
using System.Linq;

public class RitualNotes : MonoBehaviour {
    public static RitualNotes instance;

    TextMesh nameText;
    Transform ritualDisplay;
    GameObject componentVisual;

	// Use this for initialization
	public void Initialize() {
        nameText = transform.FindChild("Title").GetComponent<TextMesh>();
        ritualDisplay = transform.FindChild("RitualDisplay");
	}

    public void TryLoad()
    {
        //if there is a bookmarkedcreature and that creature has a ritual requirement that matches
        if (BookmarkedPanel.BookmarkedCreature == null) return;

        RitualComponent requiredComponent = BookmarkedPanel.BookmarkedCreature.GetFirstComponentOfType(Ritual.CurrentRitual.GetRitualType());
        if (requiredComponent == null) return;

        if (componentVisual != null)
            Destroy(componentVisual);
        nameText.text = BookmarkedPanel.BookmarkedCreature.Title;
        componentVisual = requiredComponent.GetComponentVisual();
        componentVisual.transform.parent = ritualDisplay;
        componentVisual.transform.localPosition = new Vector3(0, 0, transform.localPosition.z - 1);

        gameObject.SetActive(true);
    }
}
