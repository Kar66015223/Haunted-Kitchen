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

    void OnDisable()
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
        Time.timeScale = 0f;
        root.SetActive(true);
    }

    private void Retry()
    {
        DayManager.Instance.ResetDay();
        root.SetActive(false);
    }
    
    private void ReturnToStartScene()
    {
        Time.timeScale = 1f;
        SceneLoader.ChangeScene(SceneConstants.STARTSCENE_NAME);
    }
}
