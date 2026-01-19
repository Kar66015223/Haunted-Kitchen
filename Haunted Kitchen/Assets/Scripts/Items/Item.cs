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
        Debug.Log($"Picking up {gameObject.name}");
    }
}
