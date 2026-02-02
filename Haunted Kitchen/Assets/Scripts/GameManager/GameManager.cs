using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerMoney playerMoney;

    [Header("Money")]
    public int money;

    [Header("UI")]
    public TMP_Text moneyUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerMoney = FindAnyObjectByType<PlayerMoney>();
    }

    private void Update()
    {
        if (playerMoney != null)
        {
            money = playerMoney.currentMoney;
            UpdateMoneyUI();
        }
    }

    public void UpdateMoneyUI()
    {
        moneyUI.text = $"Money: {money} $";
    }
}
