using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float remainingTime = 0;
    [SerializeField] private float maxTime = 181;

    [SerializeField] private float ghostPhaseStartTime = 150; //2:30 Min

    [SerializeField] private Button endDayButton;
    [SerializeField] private Button startGhostPhaseButton;

    private bool hasRunOut = false;

    public event Action OnTimerRunOut;

    void Start()
    {
        ResetTime();

        if (endDayButton != null)
            endDayButton.onClick.AddListener(SetRunOut);

        if (startGhostPhaseButton != null)
            startGhostPhaseButton.onClick.AddListener(SetSpawnGhost);
    }

    void OnDestroy()
    {
        if (endDayButton != null)
            endDayButton.onClick.RemoveAllListeners();

        if (startGhostPhaseButton != null)
            startGhostPhaseButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (!hasRunOut)
        {
            remainingTime = 0;
            hasRunOut = true;
            OnTimerRunOut?.Invoke();
        }

        float ghostPhaseEndTime = ghostPhaseStartTime - 30;

        if (remainingTime <= ghostPhaseStartTime &&
            remainingTime > ghostPhaseEndTime)
        {
            GameEvents.OnToggleGhostSpawning?.Invoke(true);
        }
        else
        {
            GameEvents.OnToggleGhostSpawning?.Invoke(false);
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

    public void SetSpawnGhost()
    {
        float ghostPhaseEndTime = ghostPhaseStartTime - 30;

        if (remainingTime > ghostPhaseStartTime)
        {
            remainingTime = ghostPhaseStartTime;
        }
        else if(remainingTime <= ghostPhaseStartTime && remainingTime > ghostPhaseEndTime)
        {
            remainingTime = ghostPhaseEndTime;
        }
    }
}
