using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public ItemData itemData;
    public GameObject itemPrefab;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;

    private ShopUI shopUI;

    public Transform spawner;

    private void Start()
    {
        shopUI = GetComponentInParent<ShopUI>();

        nameText.text = itemData.itemName;

        priceText.text = $"{itemData.price} $";

        buyButton.onClick.AddListener(Buy);
    }

    void Buy()
    {
        SpawnItem();
    }

    void SpawnItem()
    {
        GameObject obj = Instantiate(itemPrefab, spawner);
    }
}
