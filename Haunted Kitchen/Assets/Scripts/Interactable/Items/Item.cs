using System;
using UnityEngine;

public class Item : MonoBehaviour, Iinteractable, IWorkerInteractable
{
    public ItemData itemData;
    public ItemState itemState;

    public event Action<IWorkerInteractable> OnFinished;

    public bool IsTargeted { get; set; }

    void Update()
    {
        if (itemData is FoodData && itemState == ItemState.NotHeld)
        {
            OnDiscovered();
        }
    }

    public virtual bool CanInteract(Interactor interactor)
    {
        if (interactor == null)
        {
            return false;
        }

        if (interactor.interactionType == InteractionType.Hold)
        {
            return false;
        }

        var playerItem = interactor.playerItem;
        if(playerItem == null)
        {
            return false;
        }

        if(itemState == ItemState.Held)
        {
            return false;
        }

        if (playerItem.currentHeldItemObj != null)
        {
            return false;
        }

        return true;
    }

    public virtual void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;
        var currentTable = interactor.currentTable;

        if (playerItem == null) return;

        playerItem.PickUp(itemData, gameObject);
        currentTable?.SetItem(null);

        Debug.Log($"Interacted with item {itemData.itemName} by {interactor.source.name}");
    }

    public void OnDiscovered() => WorkerEvents.NotifyTaskDiscovered(this);

    public Transform GetPosition() => transform;
}
