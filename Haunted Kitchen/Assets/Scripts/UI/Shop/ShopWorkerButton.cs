using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopWorkerButton : MonoBehaviour
{
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform spawner;

    [SerializeField] private GameObject spawnedWorker;

    [SerializeField] private int workerPrice = 1000;
    [SerializeField] private TMP_Text priceText;
    private Button buyButton;

    void Awake()
    {
        buyButton = GetComponent<Button>();
    }

    private void Start()
    {
        priceText.text = workerPrice.ToString();
        buyButton.onClick.AddListener(Hire);
    }

    void Hire()
    {
        if (MoneyManager.Instance.CurrentMoney < workerPrice)
        {
            GameEvents.OnShowEventText?.Invoke($"You don't have enough money.", Color.red);
            return;
        }

        if(spawnedWorker != null)
        {
            GameEvents.OnShowEventText?.Invoke($"Can only hire one worker", Color.red);
            return;
        }

        MoneyManager.Instance.ChangeMoneyAmount(-workerPrice);
        GameEvents.OnShowEventText?.Invoke($"Hired Worker", Color.green);

        SpawnWorker();
    }

    void SpawnWorker()
    {
        spawnedWorker = Instantiate(workerPrefab, spawner.position, spawner.rotation);
        spawnedWorker.GetComponent<Worker>().SetIdlePoint(spawner);
    }
}
