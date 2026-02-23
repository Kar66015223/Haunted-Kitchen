using UnityEngine;
using System;

public class CustomerPatience : MonoBehaviour
{
    public float patienceDuration = 60f;
    private float patienceTimer;
    private bool isCountingPatience = false;

    public event Action OnPatienceExpired;
    public event Action<float> OnPatienceChanged;

    private void Update()
    {
        HandlePatience();
    }

    public void StartPatienceTimer()
    {
        patienceTimer = patienceDuration;
        isCountingPatience = true;

        OnPatienceChanged?.Invoke(1f);
    }


    public void HandlePatience()
    {
        if (!isCountingPatience) return;

        patienceTimer -= Time.deltaTime;

        float normalized = patienceTimer / patienceDuration;
        OnPatienceChanged?.Invoke(normalized);

        if (patienceTimer <= 0f)
        {
            isCountingPatience = false;
            OnPatienceExpired?.Invoke();

            Debug.Log("Customer lost patience.");
        }
    }
}
