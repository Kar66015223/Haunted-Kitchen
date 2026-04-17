using System.Collections;
using UnityEngine;

public class AngryBehavior : CustomerBehavior
{
    [SerializeField] private float disappearDelay = 0.5f;
    [SerializeField] private float stunDuration = 1f;

    private bool hasAttacked;
    private bool isCorrectServe;

    public override void HandleLeaving(Customer_New customer)
    {
        if (isCorrectServe)
        {
            customer.movement.HandleLeaving();
            return;
        }
        
        AttackPlayer(customer);
    }

    public override void OnCorrectServe(Customer_New customer, int totalPrice)
    {
        int bonus = totalPrice * 2;
        isCorrectServe = true;

        MoneyManager.Instance.ChangeMoneyAmount(bonus);
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

    void AttackPlayer(Customer_New customer)
    {
        if (hasAttacked) return;
        hasAttacked = true;

        customer.movement.LeaveChair();

        PlayerController_New player = FindAnyObjectByType<PlayerController_New>();

        if (player == null)
        {
            Debug.LogError("No player found");
            return;
        }

        Vector3 offset = (customer.transform.position - player.transform.position).normalized;
        customer.transform.position = player.transform.position + offset * 1.2f;
        customer.transform.LookAt(player.transform);

        customer.movement.PlayIdle();
        customer.movement.PlayAttack();

        customer.StartCoroutine(AttackSequence(customer, player));
    }

    private IEnumerator AttackSequence(Customer_New customer, PlayerController_New controller)
    {           
        float impactDelay = 1f;

        yield return new WaitForSeconds(impactDelay);

        controller.Slip(stunDuration + impactDelay);
        customer.PlaySound();

        PlayerHealth health = controller.GetComponent<PlayerHealth>();
        health.TakeDamage(1);

        yield return new WaitForSeconds(disappearDelay);
        customer.movement.OnLeft?.Invoke();

        Destroy(customer.gameObject);
    }
}
