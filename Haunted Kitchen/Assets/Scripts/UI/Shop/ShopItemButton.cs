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

    [SerializeField] private PlayerMoney playerMoney;
    [SerializeField] private EventTextUI eventText;

    private void Start()
    {
        if (playerMoney == null)
            playerMoney = FindAnyObjectByType<PlayerMoney>();

        if (eventText == null)
            eventText = FindAnyObjectByType<EventTextUI>();

        nameText.text = itemData.itemName;
        priceText.text = $"{itemData.price} $";

        buyButton.onClick.AddListener(Buy);
    }

    void Buy()
    {
        if (playerMoney.currentMoney < itemData.price)
        {
            eventText.ShowEvent($"You don't have enough money.", Color.red);
            return;
        }

        playerMoney.ChangeMoneyAmount(-itemData.price);
        eventText.ShowEvent($"Bought {itemData.itemName}", Color.green);

        SpawnItem();
    }

    void SpawnItem()
    {
        Instantiate(itemPrefab, spawner.position, spawner.rotation);
    }
}
