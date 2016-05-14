using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System;
using System.Linq;

/// <summary>
/// Provides the necessary code to view the current order 
/// the player should be working towards fulfilling
/// </summary>
public class ViewOrder : Ritual
{
    public GameObject orderText;
    public GameObject creatureSelectPrefab;
    public GameObject componentPrefab;

    // Offsets for the submitted component information
    public float componentXOffset = .7f;
    public float componentYOffset = 2f;
    public float componentYBetween = 4f;

    // Offsets for the creatures ready to be summoned
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

    /// <summary>
    /// Reset the display to show updated order information
    /// </summary>
    public void UpdateDisplay()
    {
        // Remove all previous components and creatures
        foreach (Transform t in transform)
            if (t.tag == "Component") GameObject.Destroy(t.gameObject);

        // Display the current required attribute
        orderText.GetComponent<TextMesh>().text = "Order";

        // Display each creature with the required attribute and ready to be summoned
        int i = 0;
        foreach (Creature c in Creature.loadedCreatures.Where(c => Order.CurrentOrder.RequiredCreatures.Any(req => req.CreatureMatches(c)) &&
            c.fulfillsRequirements(Order.SubmittedComponents)))
        {
            GameObject creatureSelect = GameObject.Instantiate(creatureSelectPrefab);

            // Assign components to variables
            Transform t = creatureSelect.transform;
            MeshRenderer ren = creatureSelect.GetComponent<MeshRenderer>();
            BoxCollider2D coll = creatureSelect.GetComponent<BoxCollider2D>();

            t.parent = transform;
            creatureSelect.GetComponent<TextMesh>().text = c.Title;

            // Position the creature using the provided offsets
            t.position = new Vector3(creatureXOffset, creatureYOffset - creatureYBetween * i);

            // Position and resize the collider across the entirety of the name to allow selection
            coll.size = new Vector2(ren.bounds.size.x / t.localScale.x, ren.bounds.size.y / t.localScale.y);
            coll.offset = new Vector2(coll.size.x / 2, 0);

            // Set the current creature to be the one for submission
            creatureSelect.GetComponent<SelectCreature>().attachedCreature = c;

            // Move to the next row
            i++;
        }

        // Display each component that has been submitted
        i = 0;
        foreach (RitualComponent c in Order.SubmittedComponents)
        {
            GameObject componentInfo = GameObject.Instantiate(componentPrefab);
            componentInfo.transform.parent = transform;
            componentInfo.transform.position = new Vector3(componentXOffset, componentYOffset - componentYBetween * i);
            componentInfo.GetComponent<SpriteRenderer>().enabled = true;

            GameObject ritualDisplay = c.GetComponentVisual();
            ritualDisplay.transform.parent = componentInfo.transform.FindChild("RitualDisplay");
            ritualDisplay.transform.localPosition = new Vector3(0, 0, transform.localPosition.z - 1);

            // Move to the next row
            i++;
        }
    }
}
