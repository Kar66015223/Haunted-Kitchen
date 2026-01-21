using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class MakingBurger : MonoBehaviour, Iinteractable
{
    public RecipeData recipe;
    public Transform stackRoot;
    public float stackHeight = 0.5f;

    public int currentStepIndex = 0;
    private List<GameObject> stackedItems = new();

    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        if (playerItem == null || playerItem.currentHeldItemObj == null) return;

        Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
        IngredientData ingredient = heldItem.itemData as IngredientData;

        if (ingredient == null)
        {
            Debug.Log("not an ingredient");
            return;
        }

        if (!IsCorrectIngredient(ingredient))
        {
            Debug.Log("wrong ingredient order");
            return;
        }

        AddIngredient(playerItem, heldItem);
    }

    bool IsCorrectIngredient(IngredientData ingredient)
    {
        if (currentStepIndex >= recipe.steps.Count) return false;
        
        return recipe.steps[currentStepIndex].ingredient == ingredient; 
    }

    void AddIngredient(PlayerItem playerItem, Item item)
    {
        // parent item to stackRoot
        playerItem.DropItem();

        Transform itemTransform = item.transform;
        itemTransform.SetParent(stackRoot);

        // place item above each other
        float yOffset = stackedItems.Count * stackHeight;
        itemTransform.localPosition = new Vector3(0, yOffset, 0);
        itemTransform.localRotation = Quaternion.identity;

        if (item.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        // add item to stackedItem 
        stackedItems.Add(item.gameObject);
        currentStepIndex++;

        if (currentStepIndex >= recipe.steps.Count)
        {
            CompleteRecipe();
        }
    }

    void CompleteRecipe()
    {
        Debug.Log("burger completed");
    }
}
