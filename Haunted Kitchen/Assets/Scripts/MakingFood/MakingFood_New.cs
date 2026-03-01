using System.Collections.Generic;
using UnityEngine;

public class MakingFood_New : MonoBehaviour, Iinteractable
{
    public RecipeData recipe;

    public int currentStepIndex = 0;

    [SerializeField] private List<IngredientVisual> ingredientVisuals = new();
    private Dictionary<IngredientData, GameObject> visualLookup;

    [SerializeField] private ItemData resultItem;

    private bool isCompleted = false;

    private void Awake()
    {
        visualLookup = new Dictionary<IngredientData, GameObject>();

        foreach (var pair in ingredientVisuals)
        {
            if (!visualLookup.ContainsKey(pair.ingredient))
            {
                visualLookup.Add(pair.ingredient, pair.visual);
                pair.visual.SetActive(false);
            }
        }

        resultItem = recipe.result;
    }

    public bool CanInteract(Interactor interactor)
    {
        if (interactor.interactionType == InteractionType.Hold)
            return false;

        if (interactor?.currentTable != null && !interactor.currentTable.AllowsStationInteraction)
            return false;

        if (isCompleted) 
            return false;

        if(interactor == null) 
            return false;

        var playerItem = interactor.playerItem;

        //If player has nothing, allow pickup
        if (playerItem?.currentHeldItemObj == null)
        {
            if (interactor.currentTable == null)
                return false;

            return true;
        }

        //If player has something, check for correct ingredient and containers
        var heldObj = playerItem.currentHeldItemObj;
        if (heldObj == null)
            return false;

        //Ingredient case
        Item heldItem = heldObj.GetComponent<Item>();
        IngredientData ingredient = heldItem?.itemData as IngredientData;
        if (ingredient != null)
        {
            return IsCorrectIngredient(ingredient);
        }

        //Container case: If container content is the correct ingredient
        var container = heldObj.GetComponent<ContainerItem>();
        if (container != null)
        {
            var containerIngredient = container.ContainerContentAsIngredient();
            if (containerIngredient != null)
            {
                return IsCorrectIngredient(containerIngredient);
            }
        }

        return false;
    }

    public void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;

        if (playerItem != null &&
            playerItem.currentHeldItemObj != null)
        {
            #region Add ingredient: If player has something, check if it's the correct ingredient
            Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
            var ingredient = heldItem?.itemData as IngredientData;

            if (ingredient != null)
            {
                if (!IsCorrectIngredient(ingredient))
                {
                    Debug.Log("wrong ingredient order");
                    return;
                }

                AddIngredient(playerItem, heldItem);
                return;
            } 
            #endregion

            #region Container: If player is holding container, release content
            var container = playerItem.currentHeldItemObj.GetComponent<ContainerItem>();
            if (container != null)
            {
                var containerIngredient = container.ContainerContentAsIngredient();
                if (containerIngredient == null)
                {
                    Debug.Log("container has no ingredient content");
                    return;
                }

                if (!IsCorrectIngredient(containerIngredient))
                {
                    Debug.Log("wrong ingredient order (container)");
                    return;
                }

                Table table = interactor.currentTable ?? transform.parent?.GetComponent<Table>();

                var spawnedContent = container.ReleaseToTable(table);
                AddIngredient(null, spawnedContent.GetComponent<Item>());

                return;
            } 
            #endregion
        }

        //Pickup: If player has nothing, pickup
        if (playerItem != null &&
            playerItem.currentHeldItemObj == null)
        {
            Item item = GetComponent<Item>();

            playerItem.PickUp(item.itemData, item.gameObject);

            if (interactor.currentTable != null)
            {
                interactor.currentTable.SetItem(null);
            }

            Debug.Log($"Picked up {item.itemData.itemName} from table");
        }
    }

    bool IsCorrectIngredient(IngredientData ingredient)
    {
        if (currentStepIndex >= recipe.steps.Count) return false;

        return recipe.steps[currentStepIndex].ingredient == ingredient;
    }

    void AddIngredient(PlayerItem playerItem, Item item)
    {
        // parent item to stackRoot
        if (playerItem != null)
        {
            playerItem.DropItemNoRaycast();
        }

        IngredientData ingredient = item.itemData as IngredientData;
        Destroy(item.gameObject);

        //enable the correct visual
        if (visualLookup.TryGetValue(ingredient, out GameObject visual))
        {
            visual.SetActive(true);
        }

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

        Item item = GetComponent<Item>();
        item.itemData = resultItem;

        Table table = transform.parent.GetComponent<Table>();
        if (table != null)
        {
            table.SetItem(item);
        }

        Debug.Log("Food completed");
    }
}
