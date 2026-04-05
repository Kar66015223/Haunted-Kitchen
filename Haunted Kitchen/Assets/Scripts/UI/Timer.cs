using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float remainingTime = 0;
    [SerializeField] private float maxTime = 181;

    [SerializeField] private Button endDayButton;

    private bool hasRunOut = false;

    public event Action OnTimerRunOut;

    void Start()
    {
        ResetTime();

        if (endDayButton != null)
            endDayButton.onClick.AddListener(SetRunOut);
    }

    void OnDestroy()
    {
        if (endDayButton != null)
            endDayButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }

        else if(!hasRunOut)
        {
            remainingTime = 0;
            hasRunOut = true;
            OnTimerRunOut?.Invoke();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTime()
    {
        remainingTime = maxTime;
        hasRunOut = false;
    }

    public void SetRunOut()
    {
        remainingTime = 0;
    }
}
