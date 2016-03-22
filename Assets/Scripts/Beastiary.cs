using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

/// <summary>
/// Handles viewing information on monsters and their required componenets
/// </summary>
public class Beastiary : Ritual {
    public GameObject titleText;
    public GameObject typeText;
    public GameObject attributeText;
    public GameObject componentPrefab;
    public GameObject creatureImage;
    public Button targetCreatureButton;

    // Offsets for displaying required components
    public float xOffset = .25f;
    public float yOffset = 4f;
    public float yBetween = 1.5f;

    private int currentCreatureIndex = 0;
    private Creature currentCreature;


    // Use this for initialization
    void Start()
    {
        targetCreatureButton.onClick.AddListener(() => SetTargetCreature());
        this.currentCreature = Creature.loadedCreatures[currentCreatureIndex];
        this.displayInfo();
    }

    public override void ShowRitual()
    {
        base.ShowRitual();
        updateInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClosing())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Z))
            changePage(-1);
        else if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.X))
            changePage(1);
    }

    /// <summary>
    /// Flip to the next or previous page if possible
    /// </summary>
    /// <param name="dir">1 or -1 to determine the direction the page is turned</param>
    void changePage(int dir)
    {
        if (currentCreatureIndex + dir < 0 || currentCreatureIndex + dir >= Creature.loadedCreatures.Count)
            return;

        this.currentCreatureIndex += dir;
        currentCreature = Creature.loadedCreatures[currentCreatureIndex];
        displayInfo();

    }

    /// <summary>
    /// Update whether a required component has been submitted or not
    /// </summary>
    void updateInfo()
    {
        int i = 0;
        foreach (Transform t in transform)
            if (t.tag == "Component")
            {
                // If a required component has been submitted, add a check mark
                t.gameObject.GetComponent<SpriteRenderer>().enabled = Order.SubmittedComponents.Exists(
                    c => c.MatchesComponent(currentCreature.RequiredComponents[i], currentCreature.Names));
                i++;
            }
    }

    /// <summary>
    /// Show a new creature's information
    /// </summary>
    void displayInfo()
    {
        // Remove the components from the previous creature
        foreach (Transform t in transform)
            if (t.tag == "Component") GameObject.Destroy(t.gameObject);

        titleText.GetComponent<TextMesh>().text = currentCreature.Title;
        typeText.GetComponent<TextMesh>().text = currentCreature.Type;

        // TODO
        // Text wrapping for longer attributes
        string attributes = currentCreature.Attributes[0];
        for (int i = 1; i < currentCreature.Attributes.Count; i++)
            attributes += ", " + currentCreature.Attributes[i];
        attributeText.GetComponent<TextMesh>().text = attributes;

        // Display each required component
        for (int i = 0; i < currentCreature.RequiredComponents.Count; i++)
        {
            GameObject componentInfo = GameObject.Instantiate(componentPrefab);
            componentInfo.transform.parent = transform;
            componentInfo.transform.position = new Vector3(xOffset, yOffset - yBetween * i);
            componentInfo.transform.FindChild("Type Text").GetComponent<TextMesh>().text = currentCreature.RequiredComponents[i].ComponentType.ToString();
            componentInfo.transform.FindChild("Content Text").GetComponent<TextMesh>().text = currentCreature.RequiredComponents[i].GetContent();
            componentInfo.GetComponent<SpriteRenderer>().enabled = Order.SubmittedComponents.Exists(c => c.MatchesComponent(currentCreature.RequiredComponents[i], currentCreature.Names));
        }
        creatureImage.GetComponent<SpriteRenderer>().sprite = currentCreature.FetchCreatureSprite();
    }

    private void SetTargetCreature()
    {
        BookmarkedPanel.BookmarkedCreature = currentCreature;
        base.CloseRitual();
    }

    protected override Component GetCurrentComponent()
    {
        throw new NotImplementedException();
    }
}
