using UnityEngine;

public class NormalBehavior : CustomerBehavior
{
    public override void OnCorrectServe(Customer_New customer, int totalPrice)
    {
        MoneyManager.Instance.ChangeMoneyAmount(totalPrice);
    }

    public override void OnPatienceExpired(Customer_New customer)
    {
        int steal = Random.Range(300, 1000);
        MoneyManager.Instance.ChangeMoneyAmount(-steal);

        GameEvents.OnShowEventText?.Invoke(CustomerConstants.STEAL_EVENT_TEXT, Color.red);
    }

    public override void OnWrongServe(Customer_New customer)
    {
        int steal = Random.Range(300, 1000);
        MoneyManager.Instance.ChangeMoneyAmount(-steal);

        GameEvents.OnShowEventText?.Invoke(CustomerConstants.STEAL_EVENT_TEXT, Color.red);
    }

    public override void HandleLeaving(Customer_New customer)
    {
        customer.movement.HandleLeaving();
    }
}
