using UnityEngine;

public class TrollBehavior : CustomerBehavior
{
    [SerializeField] private int moneyReward = 2000;

    private EventTextUI eventText;
    private PlayerMoney playerMoney;

    void Start()
    {
        eventText = FindAnyObjectByType<EventTextUI>();
        playerMoney = FindAnyObjectByType<PlayerMoney>();
    }
    
    public override void HandleLeaving(Customer_New customer)
    {
        customer.movement.HandleLeaving();
    }

    public override void OnCorrectServe(Customer_New customer, int totalPrice)
    {
        if(playerMoney != null)
        {
            playerMoney.ChangeMoneyAmount(moneyReward);
        }
        
        if (eventText != null)
            eventText.ShowEvent("The troll customer rewards you after being served the right order.", Color.green);

        GameEvents.OnSpeedBuff?.Invoke(10f);

        //Spawn rune stone on the table

        Debug.Log("Player get 2000$, rune stone and 10 sec speed buff");
    }

    public override void OnPatienceExpired(Customer_New customer)
    {
        
    }

    public override void OnWrongServe(Customer_New customer)
    {
        
    }
}
