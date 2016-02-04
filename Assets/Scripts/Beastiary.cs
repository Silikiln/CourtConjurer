using UnityEngine;
using System.Linq;
using System;

public class Beastiary : Ritual {
    public GameObject titleText;
    public GameObject typeText;
    public GameObject attributeText;
    public GameObject componentPrefab;

    public float xOffset = .25f;
    public float yOffset = 4f;
    public float yBetween = 1.5f;

    private float scrollRate = 1.0f;
    private float nextScroll;
    private int currentCreatureIndex = 0;
    private Creature currentCreature;


    // Use this for initialization
    void Start()
    {
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

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Z) && Time.time > nextScroll)
        {
            nextScroll = Time.time + scrollRate;
            changePage(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.X) && Time.time > nextScroll)
        {
            nextScroll = Time.time + scrollRate;
            changePage(1);
        }
    }

    void changePage(int dir)
    {
        if (currentCreatureIndex + dir < 0 || currentCreatureIndex + dir >= Creature.loadedCreatures.Count)
            return;

        this.currentCreatureIndex += dir;
        currentCreature = Creature.loadedCreatures[currentCreatureIndex];
        displayInfo();

    }

    void updateInfo()
    {
        int i = 0;
        foreach (Transform t in transform)
            if (t.tag == "Component")
            {
                t.gameObject.GetComponent<SpriteRenderer>().enabled = Order.SubmittedComponents.Exists(
                    c => c.MatchesComponent(currentCreature.RequiredComponents[i], currentCreature.Names));
                i++;
            }
    }

    void displayInfo()
    {
        foreach (Transform t in transform)
            if (t.tag == "Component") GameObject.Destroy(t.gameObject);

        titleText.GetComponent<TextMesh>().text = currentCreature.Title;
        typeText.GetComponent<TextMesh>().text = currentCreature.Type;

        string attributes = currentCreature.Attributes[0];
        for (int i = 1; i < currentCreature.Attributes.Count; i++)
            attributes += ", " + currentCreature.Attributes[i];
        attributeText.GetComponent<TextMesh>().text = attributes;

        for (int i = 0; i < currentCreature.RequiredComponents.Count; i++)
        {
            GameObject componentInfo = GameObject.Instantiate(componentPrefab);
            componentInfo.transform.parent = transform;
            componentInfo.transform.position = new Vector3(xOffset, yOffset - yBetween * i);
            componentInfo.transform.FindChild("Type Text").GetComponent<TextMesh>().text = currentCreature.RequiredComponents[i].ComponentType.ToString();
            componentInfo.transform.FindChild("Content Text").GetComponent<TextMesh>().text = currentCreature.RequiredComponents[i].GetContent();
            componentInfo.GetComponent<SpriteRenderer>().enabled = Order.SubmittedComponents.Exists(c => c.MatchesComponent(currentCreature.RequiredComponents[i], currentCreature.Names));
        }
    }

    protected override Component GetCurrentComponent()
    {
        throw new NotImplementedException();
    }
}
