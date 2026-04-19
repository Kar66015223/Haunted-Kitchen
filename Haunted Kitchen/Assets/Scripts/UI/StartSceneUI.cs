using UnityEngine;
using UnityEngine.UI;

public class StartSceneUI : MonoBehaviour
{
    public Button startButton;
    public Button settingButton;
    public Button quitButton;

    private void Start()
    {
        startButton?.onClick.AddListener(StartGame);
        startButton?.onClick.AddListener(UISound.instance.PlayClickSound);

        settingButton?.onClick.AddListener(UISound.instance.PlayClickSound);

        quitButton?.onClick.AddListener(QuitGame);
        quitButton?.onClick.AddListener(UISound.instance.PlayClickSound);
    }

    private void StartGame()
    {
        SceneLoader.ChangeScene(SceneConstants.STORY_NAME);
    }

    private void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
