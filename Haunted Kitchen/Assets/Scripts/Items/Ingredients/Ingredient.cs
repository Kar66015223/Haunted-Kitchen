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

            transform.SetParent(interactor.transform);
            transform.localPosition = new Vector3(0, 0, 1);
            transform.localRotation = Quaternion.identity;

            Debug.Log($"Picking up {gameObject.name}"); 
        }
    }
}
