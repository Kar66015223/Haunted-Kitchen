using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public List<GameObject> storyPages;
    private int currentIndex = 0;

    public Button nextButton;
    public Button mainMenuButton;

    public Button nextDialogue;

    void Start()
    {
        ShowPage(0);

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(GoToMainGame);
            nextButton.onClick.AddListener(UISound.instance.PlayClickSound);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(ReturnToMenu);
            mainMenuButton.onClick.AddListener(UISound.instance.PlayClickSound);
        }

        if (nextDialogue != null)
            nextDialogue.onClick.AddListener(UISound.instance.PlayClickSound);
    }

    void OnDestroy()
    {
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveAllListeners();

        if (nextButton != null)
            nextButton.onClick.RemoveAllListeners();
    }

    // ��ǹ������������: �礡�á������ء� Frame
    void Update()
    {
        // �����觢ͧ�к�����
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextPage();
        }
    }

    public void NextPage()
    {
        if (currentIndex < storyPages.Count - 1)
        {
            currentIndex++;
            ShowPage(currentIndex);
        }
        else
        {
            Debug.Log("����������ͧ����!");
        }
    }

    void ShowPage(int index)
    {
        for (int i = 0; i < storyPages.Count; i++)
        {
            storyPages[i].SetActive(i == index);
        }
    }

    void GoToMainGame()
    {
        SceneLoader.ChangeScene(SceneConstants.MAINGAME_NAME);
    }

    void ReturnToMenu()
    {
        SceneLoader.ChangeScene(SceneConstants.STARTSCENE_NAME);
    }
}