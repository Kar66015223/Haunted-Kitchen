using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public List<GameObject> storyPages;
    private int currentIndex = 0;

    public Button mainMenuButton;

    void Start()
    {
        ShowPage(0);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMenu);
    }

    void OnDestroy()
    {
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveAllListeners();
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

    void ReturnToMenu()
    {
        SceneLoader.ChangeScene(SceneConstants.STARTSCENE_NAME);
    }
}