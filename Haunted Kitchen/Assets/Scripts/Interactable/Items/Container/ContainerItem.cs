using UnityEngine;

public class ContainerItem : MonoBehaviour, IContainerInteractable
{
    [SerializeField] private Item item;
    [SerializeField] private ContainerData containerData;

    private void Awake()
    {
        item = GetComponent<Item>();
        containerData = item.itemData as ContainerData;
    }

    public bool HandleContainerInteraction(GameObject interactor, Table table)
    {
        if (containerData == null || containerData.content == null)
            return false;

        SpawnContent(table);

        return true;
    }

    public GameObject SpawnContent(Table table)
    {
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

    public IngredientData ContainerContentAsIngredient()
    {
        return containerData.content
            ?.GetComponent<Item>()
            ?.itemData as IngredientData;
    }
}
