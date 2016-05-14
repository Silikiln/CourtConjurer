using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles the logic for the order specific potion ritual
/// </summary>
public class PotionRitual : Ritual {
    public int maxIngredients = 9;
    public Color neutralColor, goodColor, badColor;

    List<RitualMaterial> availableMaterials = new List<RitualMaterial>();
    List<GameObject> ingredientObjects = new List<GameObject>();

    private RitualMaterial[] correctIngredients;
    private List<byte> addedIngredients = new List<byte>();
    private KeyCode[] inputsToCheck = {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R,
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V
    };

    public Transform cauldron;
    private TextMesh ingredientText;
    private string currentIngredientString = "";

    void Start()
    {
        for (byte i = 0; i < 12; i++)
            availableMaterials.Add(RitualMaterial.Get(2, i, 0));
        ResetPotion();
    }

    public override void ShowRitual()
    {
        base.ShowRitual();
        if (BookmarkedCreatureHasComponent())
            correctIngredients = (BookmarkedCreatureComponent() as PotionComponent).RitualMaterials;
        //UpdateIngredientText();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClosing() || IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetPotion();

        if (addedIngredients.Count < maxIngredients)
            for (int i = 0; i < inputsToCheck.Length; i++)        
                if (Input.GetKeyDown(inputsToCheck[i]))
                    AddIngredient(i);        
    }

    public void AddIngredient(int ingredientIndex)
    {
        addedIngredients.Add((byte)ingredientIndex);
        canSubmit = true;
        //SetIngredientText(inputsToCheck[ingredientIndex]);

        GameObject newIngredient = availableMaterials[ingredientIndex].GetMaterialResource<GameObject>();
        newIngredient.transform.parent = cauldron;
        newIngredient.transform.localScale = new Vector3(1, 1, 1);
        ingredientObjects.Add(newIngredient);

        Vector3[] gridPositions = Grid.Generate(addedIngredients.Count, 1, 1, -1);

        for (int i = 0; i < addedIngredients.Count; i++)
            ingredientObjects[i].transform.localPosition = gridPositions[i];
    }

    /*
    void UpdateIngredientText()
    {
        if (ingredientText == null)
            ingredientText = ingredientDisplay.transform.GetComponent<TextMesh>();

        if (addedIngredients.Count == 0 || correctIngredients == null)
            ingredientText.color = neutralColor;
        else if (addedIngredients.Count <= correctIngredients.Length &&
            correctIngredients.Take(addedIngredients.Count).SequenceEqual(addedIngredients.Select(b => availableMaterials[b])))
            ingredientText.color = goodColor;
        else
            ingredientText.color = badColor;               
    }
    */

    void ResetPotion()
    {
        currentIngredientString = "";
        //ingredientText.text = ". . .";

        foreach (GameObject obj in ingredientObjects)
            Destroy(obj);

        ingredientObjects.Clear();
        addedIngredients.Clear();
        canSubmit = false;
        //UpdateIngredientText();
    }

    /*
    void SetIngredientText(KeyCode code)
    {
        currentIngredientString = currentIngredientString + code;
        ingredientText.text = currentIngredientString;
        UpdateIngredientText();
    }
    */

    protected override RitualComponent GetCurrentComponent()
    {
        List<RitualMaterial> addedMaterials = new List<RitualMaterial>();
        foreach (byte b in addedIngredients)
            addedMaterials.Add(RitualMaterial.Get(2, b, 0));
        return new PotionComponent(addedMaterials);
    }

    public override RitualComponent.Type GetRitualType()
    {
        return RitualComponent.Type.Potion;
    }
}
