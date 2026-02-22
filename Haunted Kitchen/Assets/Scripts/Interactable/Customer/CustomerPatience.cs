using UnityEngine;
using System;

public class CustomerPatience : MonoBehaviour
{
    public float patienceDuration = 60f;
    private float patienceTimer;
    private bool isCountingPatience = false;

    public event Action OnPatienceExpired;

    private void Update()
    {
        HandlePatience();
    }

    public void StartPatienceTimer()
    {
        patienceTimer = patienceDuration;
        isCountingPatience = true;

        //patienceImg.fillAmount = 1f;
    }


    public void HandlePatience()
    {
        if (!isCountingPatience) return;

        patienceTimer -= Time.deltaTime;

        //float normalized = patienceTimer / patienceDuration;
        //patienceImg.fillAmount = normalized;

        if (patienceTimer <= 0f)
        {
            isCountingPatience = false;
            OnPatienceExpired?.Invoke();

            //stealAmount = Random.Range(300, 1000);
            //GameManager.instance.playerMoney.ChangeMoneyAmount(-stealAmount);
            //GameManager.instance.ShowEventText("Your money was stolen by an angry customer...", Color.red);

            //isArrived = false;
            //exitDestinationSet = false;

            //UpdateUI();

            Debug.Log("Customer lost patience.");
        }
    }
}
