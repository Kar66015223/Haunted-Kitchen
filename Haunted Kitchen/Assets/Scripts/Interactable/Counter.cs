using UnityEngine;

public class Counter : MonoBehaviour, Iinteractable, IContextInteractable
{
    [SerializeField] private Item currentItem;
    public Transform placePoint;

    public GameObject KetchupBottlePrefab;
    public GameObject MustardBottlePrefab;

    private void Start()
    {
        if (KetchupBottlePrefab != null)
        {
            SpawnItem(KetchupBottlePrefab);
        }

        if (MustardBottlePrefab != null)
        {
            SpawnItem(MustardBottlePrefab);
        }
    }

    public bool CanInteract(PlayerItem playerItem)
    {
        if (playerItem == null)
            return false;

        // put item on table
        if (currentItem == null && playerItem.currentHeldItemObj != null)
            return true;

        // take item from table
        if (currentItem != null && playerItem.currentHeldItemObj == null)
            return true;

        return false;
    }

    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        if (playerItem == null) return;

        if (!CanInteract(playerItem))
        {
            return;
        }

        if (currentItem == null && playerItem.currentHeldItemObj != null)
        {
            PlaceItem(playerItem);
            return;
        }

        if (playerItem.currentHeldItemObj == null)
        {
            playerItem.PickUp(currentItem.itemData, currentItem.gameObject);
            currentItem = null;
        }
    }

    void PlaceItem(PlayerItem playerItem)
    {
        GameObject itemObj = playerItem.currentHeldItemObj;
        if (itemObj == null) return;

        currentItem = itemObj.GetComponent<Item>();
        if (currentItem == null) return;

        playerItem.DropItem();

        itemObj.transform.position = placePoint.position;
        itemObj.transform.rotation = placePoint.rotation;
        itemObj.transform.SetParent(transform);

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
    }

    void SpawnItem(GameObject prefab)
    {
        GameObject itemObj = Instantiate(
            prefab,
            placePoint.position,
            placePoint.rotation,
            transform
        );

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
}
