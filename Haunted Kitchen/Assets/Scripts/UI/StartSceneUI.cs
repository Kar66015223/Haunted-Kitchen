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

        quitButton?.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        SceneLoader.ChangeScene("MainGame");
    }

    private void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
