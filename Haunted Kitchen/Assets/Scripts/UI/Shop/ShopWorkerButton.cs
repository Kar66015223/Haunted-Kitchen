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
    private AudioSource source;

    void Awake()
    {
        buyButton = GetComponent<Button>();
        source = GetComponentInParent<AudioSource>();
    }

    private void Start()
    {
        priceText.text = $"{workerPrice} <color=yellow>$</color>";
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

        PlaySound();
        SpawnWorker();
    }

    void SpawnWorker()
    {
        spawnedWorker = Instantiate(workerPrefab, spawner.position, spawner.rotation);
        spawnedWorker.GetComponent<Worker>().SetIdlePoint(spawner);
    }

    void PlaySound()
    {
        if (source.isPlaying)
            source.Stop();

        source.Play();
    }
}
