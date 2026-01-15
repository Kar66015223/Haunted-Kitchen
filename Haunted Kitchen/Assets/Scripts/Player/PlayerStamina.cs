using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float drainRate = 20f;
    public float regenRate = 15f;
    public float regenDelay = 1f;

    public Image staminaBar;

    private float lastDrainTime;

    private void Update()
    {
        if (Time.time > lastDrainTime + regenDelay) // 1 second after player stop using stamina
        {
            currentStamina += regenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

            StaminaUIUpdate();
        }

        if (currentStamina == maxStamina)
        {
            staminaBar.gameObject.SetActive(false);
        }
    }

    public bool CanRun() // if currentStamina > 0, CanRun = true
    {
        return currentStamina > 0f;
    }

    public void Drain(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        lastDrainTime = Time.time;

        StaminaUIUpdate();
    }

    public void StaminaUIUpdate()
    {
        staminaBar.gameObject.SetActive(true);
        staminaBar.fillAmount = currentStamina / maxStamina;
    }
}
