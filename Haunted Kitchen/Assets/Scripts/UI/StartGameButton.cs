using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        GameEvents.OnGameStart?.Invoke();
        gameObject.SetActive(false);
    }
}
