using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyUI;
    [SerializeField] private TMP_Text moneyChangedText;
    [SerializeField] private GameObject pauseUI;

    private void Awake()
    {
        GameManager.instance.RegisterUI(this);
    }

    public TMP_Text MoneyUI => moneyUI;
    public TMP_Text MoneyChangedText => moneyChangedText;
    public GameObject PauseUI => pauseUI;
}
