using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AngryBehavior : CustomerBehavior
{
    [SerializeField] private float disappearDelay = 0.5f;
    [SerializeField] private float stunDuration = 1f;

    private bool hasAttacked;

    public override void HandleLeaving(Customer_New customer)
    {
        AttackPlayer(customer);
    }

    public override void OnCorrectServe(Customer_New customer, int totalPrice)
    {
        int bonus = totalPrice * 2;
        GameManager.instance.playerMoney.ChangeMoneyAmount(bonus);
    }

    public override void OnPatienceExpired(Customer_New customer)
    {
        int steal = UnityEngine.Random.Range(300, 1000);
        GameManager.instance.playerMoney.ChangeMoneyAmount(-steal);
        GameManager.instance.ShowEventText("Your money was stolen by an angry customer...", Color.red);
    }

    public override void OnWrongServe(Customer_New customer)
    {
        int steal = UnityEngine.Random.Range(300, 1000);
        GameManager.instance.playerMoney.ChangeMoneyAmount(-steal);
        GameManager.instance.ShowEventText("Your money was stolen by an angry customer...", Color.red);
    }

    void AttackPlayer(Customer_New customer)
    {
        if (hasAttacked) return;
        hasAttacked = true;

        PlayerController player = FindAnyObjectByType<PlayerController>();

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

    private IEnumerator AttackSequence(Customer_New customer, PlayerController controller)
    {
        float impactDelay = 0.35f;

        yield return new WaitForSeconds(impactDelay);
        controller.Slip(stunDuration);

        yield return new WaitForSeconds(disappearDelay);
        Destroy(customer.gameObject);
    }
}
