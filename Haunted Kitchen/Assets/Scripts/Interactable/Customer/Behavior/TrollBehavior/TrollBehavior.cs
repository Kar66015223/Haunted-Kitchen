using UnityEngine;

public class TrollBehavior : CustomerBehavior
{
    [SerializeField] private int moneyReward = 2000;
    
    public override void HandleLeaving(Customer_New customer)
    {
        customer.movement.HandleLeaving();
    }

    public override void OnCorrectServe(Customer_New customer, int totalPrice)
    {
        MoneyManager.Instance.ChangeMoneyAmount(moneyReward);
        
        GameEvents.OnShowEventText?.Invoke(CustomerConstants.TROLL_REWARD_EVENT_TEXT, Color.green);

        GameEvents.OnSpeedBuff?.Invoke(10f);

        if(customer.TryGetComponent(out RuneSpawner runeSpawner))
        {
            runeSpawner.SpawnRune(customer.movement.GetCurrentTable());
        }

        Debug.Log("Player get 2000$, rune stone and 10 sec speed buff");
    }

    public override void OnPatienceExpired(Customer_New customer)
    {
        
    }

    public override void OnWrongServe(Customer_New customer)
    {
        
    }
}
