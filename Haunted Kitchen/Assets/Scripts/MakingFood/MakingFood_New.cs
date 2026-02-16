using System.Collections.Generic;
using UnityEngine;

public class MakingFood_New : MonoBehaviour, Iinteractable, ITableInteractable, IStationContextInteractable
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

    public bool CanStationInteract(PlayerItem playerItem)
    {
        if (isCompleted) return false;
        if (playerItem?.currentHeldItemObj == null) return false;

        Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
        if (heldItem == null) return false;

        IngredientData ingredient = heldItem.itemData as IngredientData;
        if (ingredient == null) return false;

        return IsCorrectIngredient(ingredient);
    }

    public bool CanContainerInteract(ContainerItem container)
    {
        if (isCompleted) return false;
        if (container == null) return false;

        IngredientData ingredient = container.ContainerContentAsIngredient();

        if (ingredient == null) return false;

        return IsCorrectIngredient(ingredient);
    }

    public void HandleContainer(ContainerItem container, Table table)
    {
        if (!CanContainerInteract(container)) return;

        GameObject content = container.SpawnContent(table);

        Item spawnedItem = content.GetComponent<Item>();
        if (spawnedItem == null) return;

        AddIngredient(null, spawnedItem);
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
