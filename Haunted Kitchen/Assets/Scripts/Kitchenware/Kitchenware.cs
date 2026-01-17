using System.IO;
using UnityEngine;

public abstract class Kitchenware : MonoBehaviour, Iinteractable
{
    public string kitchenWareName;
    public KitchenwareStatus kitchenwareStatus;

    public enum KitchenwareStatus
    {
        Usable,
        Destroyed
    }

    public void Interact(GameObject interactor)
    {
        if (kitchenwareStatus == KitchenwareStatus.Usable)
        {
            Use();
        }

        Debug.Log($"{gameObject.name} interacted with by {interactor.name}");
    }

    public virtual void Use()
    {
        Debug.Log("Use() called");
    }
}
