using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene($"{_sceneName}");
    }
}