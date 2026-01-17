using UnityEngine;

public abstract class Ingredient : MonoBehaviour, Iinteractable
{
    public IngredientData ingredientData;

    public IngredientState ingredientState;

    public enum IngredientState
    {
        NotHeld,
        Held,
        Cooked
    }

    public void Interact(GameObject interactor)
    {
        if (ingredientState == IngredientState.NotHeld)
        {
            ingredientState = IngredientState.Held;

            Debug.Log($"Picking up {gameObject.name}"); 
        }
    }
}
