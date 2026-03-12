using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private bool isPaused = false;
    public bool IsPaused => isPaused;

    private PlayerInput input;

    void Start()
    {
        input = FindAnyObjectByType<PlayerInput>();
        pauseUI.SetActive(false);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMenu);
    }

    public void SetUI(GameObject pauseUI)
    {
        this.pauseUI = pauseUI;
    }

    public void Pause()
    {
        isPaused = true;
        pauseUI.SetActive(true);
        Time.timeScale = 0f;

        if (input != null)
            input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_UI);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseUI.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        if (input != null)
            input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_PLAYER);
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.ChangeScene("StartScene");
    }
}
