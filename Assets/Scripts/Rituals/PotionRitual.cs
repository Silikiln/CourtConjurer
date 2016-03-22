using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles the logic for the order specific potion ritual
/// </summary>
public class PotionRitual : Ritual {
    private byte[] neededIngredients = new byte[0];
    private byte[] addedIngredients = new byte[12];
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
        PrintWall();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClosing() || IsSubmitting())
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetPotion();

        for (int i = 0; i < inputsToCheck.Length; i++)
        {
            if(Input.GetKeyDown(inputsToCheck[i]))
            {
                MatchKey(inputsToCheck[i],i);
            }
        }
    }

    public void ButtonClick(string letter)
    {
        KeyCode clickedButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), letter);
        for (int i = 0; i < inputsToCheck.Length; i++)
        {
            if (clickedButton == inputsToCheck[i])
            {
                MatchKey(inputsToCheck[i], i);
            }
        }
    }

    void MatchKey(KeyCode keyPress, int position)
    {
        if (keyPress == inputsToCheck[position])
        {
            if (neededIngredients.Length > 0)
            {
                if (addedIngredients[position] < neededIngredients[position])
                {
                    addedIngredients[position]++;
                }
                else addedIngredients = new byte[12];
            }
            else addedIngredients[position]++;
            canSubmit = true;
            SetIngredientText(inputsToCheck[position]);
            PrintWall();
        }
    }

    void ResetPotion()
    {
        currentIngredientString = "";
        ingredientText = ingredientDisplay.transform.GetComponent<TextMesh>();
        ingredientText.text = ". . .";
        addedIngredients = new byte[12];
        canSubmit = false;
    }

    /// <summary>
    /// Prints the current state of the potion
    /// </summary>
    void PrintWall()
    {
        for (int i = 0; i < 12; i += 4)
        {
            string line = "";
            for (int x = 0; x < 4; x++)
                line += string.Format("{0}/{1}  ", addedIngredients[i + x], neededIngredients.Length > 0 ? neededIngredients[i + x].ToString() : "?");
            Debug.Log(line);
        }
    }

    protected override Component GetCurrentComponent()
    {
        return new Component(Component.Type.Potion, new List<byte>(addedIngredients));
    }
    public override Component.Type GetRitualType()
    {
        return Component.Type.Potion;
    }

    void SetIngredientText(KeyCode code)
    {
        currentIngredientString = currentIngredientString + code;
        ingredientText.text = currentIngredientString;
    }
}
