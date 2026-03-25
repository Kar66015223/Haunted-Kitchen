using TMPro;
using UnityEngine;

public class ItemAmountText : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;

    void Awake()
    {
        amountText = GetComponent<TMP_Text>();
    }

    public void UpdateAmountText(int amount)
    {
        if (amountText == null)
        {
            Debug.LogError("amountText not assigned");
        }

        amountText.text = amount.ToString();
    }
}
