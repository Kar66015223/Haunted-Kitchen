using UnityEngine;
using UnityEngine.UI;

public class PauseUIBinder : MonoBehaviour
{
    public Button resumeButton;
    public Button mainMenuButton;

    void Start()
    {
        resumeButton.onClick.AddListener(GameManager.instance.UnPause);
        mainMenuButton.onClick.AddListener(GameManager.instance.ChangeSceneToStartScene);
    }
}