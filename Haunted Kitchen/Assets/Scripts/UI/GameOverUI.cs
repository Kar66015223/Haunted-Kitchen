using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button mainMenuBtn;

    void OnEnable()
    {
        GameEvents.OnDie += TurnOnUI;
    }

    void OnDestroy()
    {
        GameEvents.OnDie -= TurnOnUI;
    }

    void Start()
    {
        if (retryBtn != null)
            retryBtn.onClick.AddListener(Retry);
        if (mainMenuBtn != null)
            mainMenuBtn.onClick.AddListener(ReturnToStartScene);
    }

    void TurnOnUI()
    {
        root.SetActive(true);
    }

    private void Retry()
    {
        DayManager.Instance.ResetDay();
        MoneyManager.Instance.ChangeMoneyAmount(-MoneyManager.Instance.CurrentMoney);

        root.SetActive(false);
    }
    
    private void ReturnToStartScene()
    {
        SceneLoader.ChangeScene(SceneConstants.STARTSCENE_NAME);
    }
}
