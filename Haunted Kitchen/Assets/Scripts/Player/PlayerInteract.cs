using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public TMP_Text interactPrompt;

    [SerializeField] private Iinteractable currentInteractable;
    private PlayerItem playerItem;

    public Iinteractable CurrentInteractable
    {
        get
        {
            return currentInteractable;
        }
    }

    private void Start()
    {
        interactPrompt.enabled = false;
        playerItem = GetComponent<PlayerItem>();
    }

    private void Update()
    {
        if (currentInteractable is IContextInteractable context)
        {
            if (!context.CanInteract(playerItem))
            {
                ClearInteractable();
            }
        }
    }

    public void TryInteract()
    {
        if (currentInteractable == null) return;

        currentInteractable.Interact(gameObject);

        // PickUp
        if (playerItem.currentHeldItemData == null) // If player's hands is free
        {
            GameObject obj = ((MonoBehaviour)currentInteractable).gameObject; // Iinteractable with Monobehaviour

            if (obj.CompareTag("Item"))
            {
                Item item = obj.GetComponent<Item>();
                if (item != null)
                {
                    playerItem.PickUp(item.itemData, item.gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Interactable") && !other.CompareTag("Item"))
            return;

        Iinteractable interactable = other.GetComponentInParent<Iinteractable>();
        if (interactable == null) return;

        if (interactable is IContextInteractable context)
        {
            if (!context.CanInteract(playerItem))
                return;
        }

        currentInteractable = interactable;
        interactPrompt.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Iinteractable interactable = other.GetComponentInParent<Iinteractable>();

        if(interactable == null) return;

        if (interactable == currentInteractable)
        {
            ClearInteractable();
        }
    }

    public void ClearInteractable()
    {
        currentInteractable = null;
        interactPrompt.enabled = false;
    }
}
