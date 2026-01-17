using UnityEngine;

public class BurgerPatty : Ingredient
{
    private void Start()
    {
        ingredientState = IngredientState.NotHeld;
    }

    private void Update()
    {
        Debug.Log(ingredientState);
    }
}
