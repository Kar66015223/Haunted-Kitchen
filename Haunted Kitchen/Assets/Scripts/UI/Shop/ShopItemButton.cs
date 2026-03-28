using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public ItemData itemData;
    public GameObject itemPrefab;
    public Transform spawner;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;

    private void Start()
    {
        nameText.text = itemData.itemName;
        priceText.text = $"{itemData.price} <color=yellow>$</color>";

        buyButton.onClick.AddListener(Buy);
    }

    void Buy()
    {
        if (MoneyManager.Instance.CurrentMoney < itemData.price)
        {
            GameEvents.OnShowEventText?.Invoke($"You don't have enough money.", Color.red);
            return;
        }

        MoneyManager.Instance.ChangeMoneyAmount(-itemData.price);
        GameEvents.OnShowEventText?.Invoke($"Bought {itemData.itemName}", Color.green);

        SpawnItem();
    }

    void SpawnItem()
    {
        Instantiate(itemPrefab, spawner.position, spawner.rotation);
    }
}
