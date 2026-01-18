using NUnit.Framework.Internal.Execution;
using UnityEngine;

public abstract class Item : MonoBehaviour, Iinteractable
{
    public ItemData itemData;
    public ItemState itemState;

    public enum ItemState
    {
        NotHeld,
        Held
    }

    public void Interact(GameObject interactor)
    {
        if (itemState == ItemState.NotHeld)
        {
            itemState = ItemState.Held;
        }

        Debug.Log($"Picking up {gameObject.name}");
    }
}
