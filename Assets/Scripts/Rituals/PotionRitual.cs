using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles the logic for the order specific potion ritual
/// </summary>
public class PotionRitual : Ritual {
    public Color neutralColor, goodColor, badColor;

    private byte[] correctIngredients;
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
        ResetPotion();
    }

    public override void ShowRitual()
    {
        base.ShowRitual();
        //correctIngredients = BookmarkedCreatureComponent();
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
            correctIngredients.Take(addedIngredients.Count).SequenceEqual(addedIngredients))
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
}
