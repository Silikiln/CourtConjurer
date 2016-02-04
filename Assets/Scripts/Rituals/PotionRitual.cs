using UnityEngine;
using System.Collections.Generic;

public class PotionRitual : Ritual {
    private byte[] neededIngredients = new byte[0];
    private byte[] addedIngredients = new byte[12];
    private KeyCode[] inputsToCheck = {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R,
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V
    };

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
            if (Input.GetKeyDown(inputsToCheck[i]))
            {
                if (neededIngredients.Length > 0)
                {
                    if (addedIngredients[i] < neededIngredients[i])
                        addedIngredients[i]++;
                    else addedIngredients = new byte[12];
                }
                else addedIngredients[i]++;
                canSubmit = true;
                PrintWall();
            }
    }

    void ResetPotion()
    {
        addedIngredients = new byte[12];
        canSubmit = false;
    }

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
}
