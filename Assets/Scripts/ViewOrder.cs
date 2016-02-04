using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System;
using System.Linq;

public class ViewOrder : Ritual
{
    public GameObject orderText;
    public GameObject creatureSelectPrefab;
    public GameObject componentPrefab;

    public float componentXOffset = .25f;
    public float componentYOffset = 2.8f;
    public float componentYBetween = 1.5f;

    public float creatureXOffset = -8;
    public float creatureYOffset = 3;
    public float creatureYBetween = 1;

    public override void ShowRitual()
    {
        base.ShowRitual();
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClosing())
            return;
    }

    public void UpdateDisplay()
    {
        foreach (Transform t in transform)
            if (t.tag == "Component") GameObject.Destroy(t.gameObject);

        orderText.GetComponent<TextMesh>().text = "Order: " + Order.CurrentOrder.RequiredAttribute;
        int i = 0;
        foreach (Creature c in Creature.loadedCreatures.Where(c => c.Attributes.Contains(Order.CurrentOrder.RequiredAttribute) &&
            c.fulfillsRequirements(Order.SubmittedComponents)))
        {
            GameObject creatureSelect = GameObject.Instantiate(creatureSelectPrefab);
            Transform t = creatureSelect.transform;
            MeshRenderer ren = creatureSelect.GetComponent<MeshRenderer>();
            BoxCollider2D coll = creatureSelect.GetComponent<BoxCollider2D>();

            t.parent = transform;
            t.position = new Vector3(creatureXOffset, creatureYOffset - creatureYBetween * i);
            creatureSelect.GetComponent<TextMesh>().text = c.Title;

            coll.size = new Vector2(ren.bounds.size.x / t.localScale.x, ren.bounds.size.y / t.localScale.y);
            coll.offset = new Vector2(coll.size.x / 2, 0);
            creatureSelect.GetComponent<SelectCreature>().attachedCreature = c;

            i++;
        }

        i = 0;
        foreach (Component c in Order.SubmittedComponents)
        {
            GameObject componentInfo = GameObject.Instantiate(componentPrefab);
            componentInfo.transform.parent = transform;
            componentInfo.transform.position = new Vector3(componentXOffset, componentYOffset - componentYBetween * i);
            componentInfo.transform.FindChild("Type Text").GetComponent<TextMesh>().text = c.ComponentType.ToString();
            componentInfo.transform.FindChild("Content Text").GetComponent<TextMesh>().text = c.GetContent();
            componentInfo.GetComponent<SpriteRenderer>().enabled = true;
            i++;
        }
    }

    protected override Component GetCurrentComponent()
    {
        throw new NotImplementedException();
    }
}
