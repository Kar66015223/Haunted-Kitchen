using System;
using UnityEngine;

public class Item : MonoBehaviour, Iinteractable, IWorkerInteractable
{
    public ItemData itemData;
    [SerializeField] private ItemState itemState;

    public event Action<IWorkerInteractable> OnFinished;
    
    public bool IsTargeted { get; set; }

    void OnDestroy()
    {
        OnFinished?.Invoke(this);
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

        IsTargeted = false;

        playerItem.PickUp(itemData, gameObject);
        currentTable?.SetItem(null);

        OnFinished?.Invoke(this);

        // Debug.Log($"Interacted with item {itemData.itemName} by {interactor.source.name}");
    }

    public void SetState(ItemState newState)
    {
        if (itemState == newState)
            return;
        
        itemState = newState;

        if (itemData is FoodData && itemState == ItemState.NotHeld)
        {
            OnDiscovered();
        }
        else
        {
            OnFinished?.Invoke(this);
        }
    }

    public ItemState GetItemState() => itemState;

    public void OnDiscovered() => WorkerEvents.NotifyTaskDiscovered(this);

    public Transform GetPosition() => transform;

    public void SetWorkerHeld()
    {
        SetState(ItemState.Held);
        OnFinished?.Invoke(this);
    }
}
