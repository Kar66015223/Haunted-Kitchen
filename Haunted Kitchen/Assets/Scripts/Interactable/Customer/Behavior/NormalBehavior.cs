using UnityEngine;

public class NormalBehavior : CustomerBehavior
{
    private EventTextUI eventText;
    private PlayerMoney playerMoney;

    void Start()
    {
        eventText = FindAnyObjectByType<EventTextUI>();
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

        if (eventText != null)
            eventText.ShowEvent("Your money was stolen by an angry customer...", Color.red);
    }

    public override void OnWrongServe(Customer_New customer)
    {
        if (playerMoney != null)
        {
            int steal = Random.Range(300, 1000);
            playerMoney.ChangeMoneyAmount(-steal);
        }

        if (eventText != null)
            eventText.ShowEvent("Your money was stolen by an angry customer...", Color.red);
    }

    public override void HandleLeaving(Customer_New customer)
    {
        customer.movement.HandleLeaving();
    }
}
