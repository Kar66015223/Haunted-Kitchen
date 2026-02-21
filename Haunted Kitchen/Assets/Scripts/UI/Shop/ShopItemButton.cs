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
            GameManager.instance.ShowEventText($"You don't have enough money.", Color.red);

            return;
        }

        GameManager.instance.playerMoney.ChangeMoneyAmount(-itemData.price);
        GameManager.instance.ShowEventText($"Bought {itemData.itemName}", Color.green);

        SpawnItem();
    }

    void SpawnItem()
    {
        GameObject obj = Instantiate(itemPrefab, spawner.position, spawner.rotation);
    }
}
