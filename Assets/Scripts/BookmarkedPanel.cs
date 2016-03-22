using UnityEngine;
using System.Collections;

public class BookmarkedPanel : MonoBehaviour {
    public static BookmarkedPanel instance;

    private static Creature _bookmarkedCreature;
    public static Creature BookmarkedCreature
    {
        get { return _bookmarkedCreature; }
        set {
            _bookmarkedCreature = value;
            instance.UpdatePanelDisplay();
        }
    }

    private TextMesh titleText, typeText;
    private SpriteRenderer creatureSprite;

    public void Initialize()
    {
        creatureSprite = transform.FindChild("CreatureImage").GetComponent<SpriteRenderer>();
        titleText = transform.FindChild("CreatureTitle").GetComponent<TextMesh>();
        typeText = transform.FindChild("CreatureType").GetComponent<TextMesh>();
    }

    private void UpdatePanelDisplay()
    {
        if (BookmarkedCreature == null)
        {
            gameObject.SetActive(false);
            return;
        }

        //set the panel parts based on the currentBookmarkedCreature
        titleText.text = BookmarkedCreature.Title;
        typeText.text = BookmarkedCreature.Type;
        creatureSprite.sprite = BookmarkedCreature.FetchCreatureSprite();
        gameObject.SetActive(true);
    }
}