using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoryController : MonoBehaviour
{
    public List<GameObject> storyPages;
    private int currentIndex = 0;

    void Start()
    {
        ShowPage(0);
    }

    // ส่วนที่เพิ่มเข้ามา: เช็คการกดปุ่มทุกๆ Frame
    void Update()
    {
        // ใช้คำสั่งของระบบใหม่
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
            Debug.Log("จบเนื้อเรื่องแล้ว!");
        }
    }

    void ShowPage(int index)
    {
        for (int i = 0; i < storyPages.Count; i++)
        {
            storyPages[i].SetActive(i == index);
        }
    }
}