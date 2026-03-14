using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EventTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text eventText;

    [SerializeField] private FadeOutText textFadeOut;
    private Coroutine fadeCo;

    void Awake()
    {
        eventText = GetComponent<TMP_Text>();
        textFadeOut = eventText.GetComponent<FadeOutText>();

        if (textFadeOut == null)
            Debug.LogError($"FadeOutText not found on {eventText.gameObject.name}");
    }

    void OnEnable()
    {
        GameEvents.OnShowEventText += ShowEvent;
    }

    void OnDisable()
    {
        GameEvents.OnShowEventText += ShowEvent;
    }

    public void SetUI(TMP_Text eventText)
    {
        this.eventText = eventText;
    }

    public void ShowEvent(string text, Color color)
    {
        eventText.text = text;
        eventText.color = color;

        if (fadeCo != null)
            StopCoroutine(fadeCo);

        if (textFadeOut != null)
        {
            textFadeOut.SetAlphaToFull();
            fadeCo = StartCoroutine(textFadeOut.FadeOut());
        }
    }
}
