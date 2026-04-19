using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<Button> allButtons = new();

    void Start()
    {
        foreach(Button button in allButtons)
        {
            button.onClick.AddListener(UISound.instance.PlayClickSound);
        }
    }
}
