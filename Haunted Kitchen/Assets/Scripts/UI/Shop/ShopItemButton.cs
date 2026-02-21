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

    public Transform spawner;

    private void Start()
    {
        nameText.text = itemData.itemName;

        priceText.text = $"{itemData.price} $";

        buyButton.onClick.AddListener(Buy);
    }

    void Buy()
    {
        if (GameManager.instance.playerMoney.currentMoney < itemData.price)
        {
            GameManager.instance.eventText.text = $"You don't have enough money.";
            GameManager.instance.eventText.color = Color.red;
            GameManager.instance.ShowEventText();

            return;
        }

        GameManager.instance.playerMoney.ChangeMoneyAmount(-itemData.price);

        GameManager.instance.eventText.text = $"Bought {itemData.itemName}";
        GameManager.instance.eventText.color = Color.green;
        GameManager.instance.ShowEventText();

        SpawnItem();
    }

    void SpawnItem()
    {
        GameObject obj = Instantiate(itemPrefab, spawner.position, spawner.rotation);
    }
}
