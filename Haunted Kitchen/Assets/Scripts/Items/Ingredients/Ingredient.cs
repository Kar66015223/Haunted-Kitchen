using UnityEngine;

public abstract class Ingredient : Item
{
    public IngredientState ingredientState;

    public enum IngredientState
    {
        NotHeld,
        Held,
        Cooked
    }
}
