    using System.IO;
    using UnityEngine;

    public class NormalBehavior : CustomerBehavior
    {
        public override void OnCorrectServe(Customer_New customer, int totalPrice)
        {
            GameManager.instance.playerMoney.ChangeMoneyAmount(totalPrice);
        }

        public override void OnPatienceExpired(Customer_New customer)
        {
            int steal = Random.Range(300, 1000);
            GameManager.instance.playerMoney.ChangeMoneyAmount(-steal);
            GameManager.instance.ShowEventText("Your money was stolen by an angry customer...", Color.red);
        }

        public override void OnWrongServe(Customer_New customer)
        {
            int steal = Random.Range(300, 1000);
            GameManager.instance.playerMoney.ChangeMoneyAmount(-steal);
            GameManager.instance.ShowEventText("Your money was stolen by an angry customer...", Color.red);
        }

        public override void HandleLeaving(Customer_New customer)
        {
            customer.movement.HandleLeaving();
        }
    }
