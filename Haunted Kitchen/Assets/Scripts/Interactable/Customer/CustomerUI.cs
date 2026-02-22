using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    [Header("Order")]
    [SerializeField] private Image idleUI;
    [SerializeField] private Image orderUI;

    [Header("Patience")]
    [SerializeField] private Image patienceImg;

    //public void UpdateUI()
    //{
    //    idleUI.gameObject.SetActive(state == CustomerState.Idle && isArrived);

    //    orderUI.gameObject.SetActive(state == CustomerState.Ordered);

    //    patienceImg.gameObject.SetActive(isArrived && state != CustomerState.Leaving);

    //    if (state == CustomerState.Ordered && orderedItem != null)
    //    {
    //        orderUI.sprite = orderedItem.icon;
    //    }
    //}
}
