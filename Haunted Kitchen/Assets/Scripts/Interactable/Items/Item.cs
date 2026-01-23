using System.Collections.Generic;
using NUnit.Framework.Internal.Execution;
using UnityEngine;

public class Item : MonoBehaviour, Iinteractable, IContextInteractable
{
    public ItemData itemData;
    public ItemState itemState;

    public enum ItemState
    {
        NotHeld,
        Held
    }

    public bool CanInteract(PlayerItem playerItem)
    {
        if (playerItem == null) 
            return false;

        if (itemState != ItemState.NotHeld)
            return false;

        if (TryGetComponent<ITableInteractable>(out _))
        {
            return true;
        }

        if (playerItem.currentHeldItemObj != null) 
            return false;

        return true;
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Picking up {gameObject.name}");
    }
}
