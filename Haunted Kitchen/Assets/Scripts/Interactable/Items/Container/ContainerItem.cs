using UnityEngine;

public class ContainerItem : MonoBehaviour, Iinteractable
{
    [SerializeField] private Item item;
    [SerializeField] private ContainerData containerData;

    private void Awake()
    {
        item = GetComponent<Item>();
        containerData = item?.itemData as ContainerData;
    }

    public bool CanInteract(Interactor interactor)
    {
        if(interactor == null)
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        var playerItem = interactor.playerItem;

        //If has currentTable & has content, allow interact
        if (interactor.currentTable != null && 
            containerData != null &&
            containerData.content != null)
            return true;

        //Allow interact as a normal item
        if(playerItem != null && playerItem.currentHeldItemObj == null)
            return true;

        return false;
    }

    public void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;
        if (playerItem == null) 
            return;

        if (playerItem != null &&
            playerItem.currentHeldItemObj == null)
        {
            Item item = GetComponent<Item>();

            playerItem.PickUp(item.itemData, item.gameObject);

            if (interactor.currentTable != null)
            {
                interactor.currentTable.SetItem(null);
            }
        }
    }

    public GameObject SpawnContent(Table table)
    {
        if (containerData == null || containerData.content == null)
            return null;

        GameObject contentObj = Instantiate(
            containerData.content,
            table.placePoint.position,
            table.placePoint.rotation
        );

        contentObj.transform.SetParent(table.transform);

        if (contentObj.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = true;

        Debug.Log("Container released its content");

        return contentObj;
    }

    public GameObject ReleaseToTable(Table table)
    {
        if (table == null) return null;
        return SpawnContent(table);
    }

    public IngredientData ContainerContentAsIngredient()
    {
        return containerData
            ?.content
            ?.GetComponent<Item>()
            ?.itemData as IngredientData;
    }
}
