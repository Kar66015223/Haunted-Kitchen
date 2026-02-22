using UnityEngine;

public abstract class CustomerBehavior : MonoBehaviour
{
    public abstract void OnPatienceExpired(Customer_New customer);
    public abstract void OnCorrectServe(Customer_New customer, int totalPrice);
    public abstract void OnWrongServe(Customer_New customer);
}
