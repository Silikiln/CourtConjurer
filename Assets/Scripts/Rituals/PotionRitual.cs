using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles the logic for the order specific potion ritual
/// </summary>
public class PotionRitual : Ritual {
    public Color neutralColor, goodColor, badColor;

    List<RitualMaterial> availableMaterials = new List<RitualMaterial>();

    private RitualMaterial[] correctIngredients;
    private List<byte> addedIngredients = new List<byte>();
    private KeyCode[] inputsToCheck = {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R,
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V
    };

    public GameObject ingredientDisplay;
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
        UpdateIngredientText();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClosing() || IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetPotion();

        for (int i = 0; i < inputsToCheck.Length; i++)        
            if (Input.GetKeyDown(inputsToCheck[i]))
                AddIngredient(i);        
    }

    public void AddIngredient(int ingredientIndex)
    {
        addedIngredients.Add((byte)ingredientIndex);
        canSubmit = true;
        SetIngredientText(inputsToCheck[ingredientIndex]);
    }

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

    void ResetPotion()
    {
        currentIngredientString = "";
        ingredientText.text = ". . .";
        addedIngredients.Clear();
        canSubmit = false;
        UpdateIngredientText();
    }

    void SetIngredientText(KeyCode code)
    {
        currentIngredientString = currentIngredientString + code;
        ingredientText.text = currentIngredientString;
        UpdateIngredientText();
    }

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
