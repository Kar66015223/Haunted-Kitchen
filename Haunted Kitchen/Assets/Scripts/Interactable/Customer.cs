using UnityEngine;

public class Customer : MonoBehaviour, Iinteractable
{
    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();

        if (playerItem.currentHeldItemObj != null)
        {
            Debug.Log($"{playerItem.currentHeldItemObj.name}");
            return;
        }

        Debug.Log($"interacting with {gameObject.name}");
    }
}
