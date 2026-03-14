using UnityEngine;

public class NormalBehavior : CustomerBehavior
{
    private PlayerMoney playerMoney;

    void Start()
    {
        playerMoney = FindAnyObjectByType<PlayerMoney>();
    }

    public override void OnCorrectServe(Customer_New customer, int totalPrice)
    {
        if(playerMoney != null)
        {
            playerMoney.ChangeMoneyAmount(totalPrice);
        }
    }

    public override void OnPatienceExpired(Customer_New customer)
    {
        if (playerMoney != null)
        {
            int steal = Random.Range(300, 1000);
            playerMoney.ChangeMoneyAmount(-steal);
        }

        GameEvents.OnShowEventText?.Invoke(CustomerConstants.STEAL_EVENT_TEXT, Color.red);
    }

    public override void OnWrongServe(Customer_New customer)
    {
        if (playerMoney != null)
        {
            int steal = Random.Range(300, 1000);
            playerMoney.ChangeMoneyAmount(-steal);
        }

        GameEvents.OnShowEventText?.Invoke(CustomerConstants.STEAL_EVENT_TEXT, Color.red);
    }

    public override void HandleLeaving(Customer_New customer)
    {
        customer.movement.HandleLeaving();
    }
}
