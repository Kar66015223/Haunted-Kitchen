using System.Collections.Generic;
using UnityEngine;

public class MakingBurger : MonoBehaviour, Iinteractable, ITableInteractable, IContextInteractable
{
    public RecipeData recipe;
    public Transform stackRoot;
    public float stackHeight = 0.5f;

    public int currentStepIndex = 0;
    private List<GameObject> stackedItems = new();

    public GameObject resultPrefab;
    private bool isCompleted = false;

    public bool CanInteract(PlayerItem playerItem)
    {
        if(isCompleted) return false;
        if(playerItem?.currentHeldItemObj == null) return false;

        Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
        if(heldItem == null) return false;

        IngredientData ingredient = heldItem.itemData as IngredientData;
        if(ingredient == null) return false;

        return IsCorrectIngredient(ingredient);
    }

    public bool HandleTableInteraction(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        if (playerItem == null) return false;

        if (playerItem.currentHeldItemObj != null)
        {
            Interact(interactor);
            return true;
        }

        return false;
    }

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
        if (isCompleted) return;
        isCompleted = true;

        Transform parent = transform.parent;
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        foreach (GameObject item in stackedItems)
        {
            Destroy(item);
        }

        GameObject resultObj = Instantiate(resultPrefab, position, rotation, parent);

        Table table = parent.GetComponent<Table>();
        if (table != null)
        {
            table.SetItem(resultObj.GetComponent<Item>());
        }

        Debug.Log("burger completed");

        Destroy(gameObject);
    }
}
