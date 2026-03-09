using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] private PlayerInteractableDetector detector;
    [SerializeField] private PlayerItem playerItem;

    void Awake()
    {
        detector = GetComponent<PlayerInteractableDetector>();
        playerItem = GetComponent<PlayerItem>();
    }

    public bool CanInteractWithCurrent()
    {
        var interactable = detector.GetCurrentInteractable();
        if (interactable == null)
            return false;

        var interactor = new Interactor(gameObject, InteractionType.Press);
        return interactable.CanInteract(interactor);
    }

    public bool CanHoldInteractWithCurrent()
    {
        var interactable = detector.GetCurrentInteractable();
        if (interactable == null)
            return false;

        var mb = detector.GetCurrentInteractableMB();
        var interactor = new Interactor(gameObject, InteractionType.Hold);

        if (mb != null)
        {
            var table = mb.GetComponent<Table>();
            if (table != null)
                interactor.currentTable = table;
        }

        return interactable.CanInteract(interactor);
    }

    public void TryInteract()
    {
        var interactable = detector.GetCurrentInteractable();
        if (interactable == null) return;

        var interactor = new Interactor(gameObject, InteractionType.Press);
        if (!interactable.CanInteract(interactor)) return;

        interactable.Interact(interactor);

        // Pick up if it's an item
        TryPickupItem(interactable);
    }

    public void TryHoldInteract()
    {
        var interactable = detector.GetCurrentInteractable();
        if (interactable == null) return;

        var mb = detector.GetCurrentInteractableMB();
        var interactor = new Interactor(gameObject, InteractionType.Hold);

        if (mb != null)
        {
            var table = mb.GetComponent<Table>();
            if (table != null)
                interactor.currentTable = table;
        }

        if (!interactable.CanInteract(interactor)) return;

        interactable.Interact(interactor);
    }
    
    private void TryPickupItem(Iinteractable interactable)
    {
        if (playerItem.currentHeldItemData != null) return;

        var mb = detector.GetCurrentInteractableMB();
        if (mb == null) return;

        var item = mb.GetComponent<Item>();
        var table = mb.GetComponent<Table>();

        // Pickup if it's an item
        if (item != null && table == null)
        {
            if (item.itemState != ItemState.Held)
            {
                playerItem.PickUp(item.itemData, item.gameObject);
            }
        }

        // Pickup item on the table if there is one
        if(table != null)
        {
            Item tableItem = table.GetCurrentItem();
            
            if(tableItem != null)
            {
                playerItem.PickUp(tableItem.itemData, tableItem.gameObject);
                table.SetItem(null);
            }
        }
    }
    
}
