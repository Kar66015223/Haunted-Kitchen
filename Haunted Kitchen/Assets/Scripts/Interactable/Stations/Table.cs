using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour, Iinteractable
{
    [SerializeField] protected Item currentItem;
    public Transform placePoint;
    public Transform customerStandPoint;

    public TableRole tableRole = TableRole.Default;

    public bool isOccupied;

    public virtual bool AllowsStationInteraction => tableRole != TableRole.Counter;

    public bool CanInteract(Interactor interactor)
    {
        if (customerStandPoint != null)
            return false;

        if (interactor == null)
            return false;

        var playerItem = interactor.playerItem;

        // If there's an item on the table, first ask the item whether it accepts the forwarded interaction.
        if (currentItem != null)
        {
            // Collect all Iinteractable implementations on the item (including Item itself)
            var interactables = currentItem
                .GetComponents<MonoBehaviour>()
                .OfType<Iinteractable>()
                .ToArray();

            // Prefer any interactable that is NOT the Item component (e.g. MakingFood_New)
            var preferred = interactables.FirstOrDefault(i => !(i is Item));
            var chosen = preferred ?? interactables.FirstOrDefault(i => i is Item);

            if (chosen != null)
            {
                var forwarded = new Interactor(interactor.source, interactor.interactionType, this);
                if (chosen.CanInteract(forwarded))
                    return true; // the item/behavior explicitly accepts this interaction (press/hold)
            }

            // If the item did NOT accept the forwarded interaction, allow the table's default pickup
            // only for Press (not for Hold).
            if (interactor.interactionType == InteractionType.Press &&
                playerItem != null &&
                playerItem.currentHeldItemObj == null)
            {
                return true;
            }

            return false;
        }

        // Table is empty: allow placing only on Press (not Hold)
        if (currentItem == null &&
            playerItem != null &&
            playerItem.currentHeldItemObj != null &&
            interactor.interactionType == InteractionType.Press)
        {
            return true;
        }

        return false;
    }

    public void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;

        // If there's an item on the table, try forwarding to the item first.
        if (currentItem != null)
        {
            var interactables = currentItem
                .GetComponents<MonoBehaviour>()
                .OfType<Iinteractable>()
                .ToArray();

            var preferred = interactables.FirstOrDefault(i => !(i is Item));
            var chosen = preferred ?? interactables.FirstOrDefault(i => i is Item);

            if (chosen != null)
            {
                var forwarded = new Interactor(interactor.source, interactor.interactionType, this);
                if (chosen.CanInteract(forwarded))
                {
                    chosen.Interact(forwarded);
                    return;
                }
            }

            // If item didn't accept forwarded interaction, fall back to default pickup � only on Press
            if (interactor.interactionType == InteractionType.Press &&
                playerItem != null &&
                playerItem.currentHeldItemObj == null)
            {
                playerItem.PickUp(currentItem.itemData, currentItem.gameObject);
                currentItem = null;
                return;
            }

            Debug.Log($"{gameObject.name} interaction not handled (item refused forwarded interaction or wrong interaction type).");
            return;
        }

        // Table empty -> placing only allowed on Press
        if (currentItem == null &&
            playerItem != null &&
            playerItem.currentHeldItemObj != null &&
            interactor.interactionType == InteractionType.Press)
        {
            PlaceItem(playerItem);
            return;
        }

        Debug.Log($"{gameObject.name} interacted by {interactor.source.name} (no action matched).");
    }

    void PlaceItem(PlayerItem playerItem)
    {
        GameObject itemObj = playerItem.currentHeldItemObj;
        if (itemObj == null) return;

        currentItem = itemObj.GetComponent<Item>();
        if (currentItem == null) return;

        playerItem.DropItemNoRaycast();
        currentItem.itemState = ItemState.NotHeld;

        itemObj.transform.position = placePoint.position;
        itemObj.transform.rotation = placePoint.rotation;
        itemObj.transform.SetParent(transform, true);

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
    }

    public void SetItem(Item item)
    {
        currentItem = item;
    }

    protected void SpawnItem(GameObject prefab)
    {
        GameObject itemObj = Instantiate(
            prefab,
            placePoint.position,
            placePoint.rotation
        );

        itemObj.transform.SetParent(transform, true);

        Item item = itemObj.GetComponent<Item>();
        if (item == null)
        {
            Debug.LogError($"{prefab.name} does not have an Item component");
            return;
        }

        currentItem = item;

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
    }

    public Item GetCurrentItem() => currentItem;
}
