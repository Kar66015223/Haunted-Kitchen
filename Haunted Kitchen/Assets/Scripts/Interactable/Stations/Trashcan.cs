using UnityEngine;

public class Trashcan : MonoBehaviour, Iinteractable
{
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public bool CanInteract(Interactor interactor)
    {
        if (interactor == null)
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        if (interactor.source == null)
            return false;

        if (interactor.playerItem == null)
            return false;

        if (interactor.playerItem.currentHeldItemData == null)
            return false;

        return interactor.playerItem.currentHeldItemData is IngredientData ||
            interactor.playerItem.currentHeldItemData is FoodData;
    }

    public void Interact(Interactor interactor)
    {
        PlayerItem playerItem = interactor.playerItem;

        if(playerItem.currentHeldItemData is IngredientData ||
            playerItem.currentHeldItemData is FoodData)
        {
            GameObject itemObj = playerItem.currentHeldItemObj;
            playerItem.DropItemNoRaycast();
            Destroy(itemObj);

            if(!source.isPlaying)
                source.Play();
        }
    }
}
