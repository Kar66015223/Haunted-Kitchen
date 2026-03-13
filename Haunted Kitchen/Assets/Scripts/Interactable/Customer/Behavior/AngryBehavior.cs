using System.Collections;
using UnityEngine;

public class AngryBehavior : CustomerBehavior
{
    [SerializeField] private float disappearDelay = 0.5f;
    [SerializeField] private float stunDuration = 1f;

    private bool hasAttacked;

    private EventTextUI eventText;
    private PlayerMoney playerMoney;

    void Start()
    {
        eventText = FindAnyObjectByType<EventTextUI>();
        playerMoney = FindAnyObjectByType<PlayerMoney>();
    }

    public override void HandleLeaving(Customer_New customer)
    {
        AttackPlayer(customer);
    }

    public override void OnCorrectServe(Customer_New customer, int totalPrice)
    {
        int bonus = totalPrice * 2;

        if(playerMoney != null)
            playerMoney.ChangeMoneyAmount(bonus);
    }

    public override void OnPatienceExpired(Customer_New customer)
    {
        if(playerMoney != null)
        {
            int steal = Random.Range(300, 1000);
            playerMoney.ChangeMoneyAmount(-steal);
        }

        if(eventText != null)
            eventText.ShowEvent("Your money was stolen by an angry customer...", Color.red);
    }

    public override void OnWrongServe(Customer_New customer)
    {
        if(playerMoney != null)
        {
            int steal = Random.Range(300, 1000);
            playerMoney.ChangeMoneyAmount(-steal);
        }

        if(eventText != null)
            eventText.ShowEvent("Your money was stolen by an angry customer...", Color.red);
    }

    void AttackPlayer(Customer_New customer)
    {
        if (hasAttacked) return;
        hasAttacked = true;

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
        float impactDelay = 0.35f;

        yield return new WaitForSeconds(impactDelay);
        controller.Slip(stunDuration + impactDelay);

        PlayerHealth health = controller.GetComponent<PlayerHealth>();
        health.TakeDamage(1);

        yield return new WaitForSeconds(disappearDelay);
        customer.movement.OnLeft?.Invoke();
        Destroy(customer.gameObject);
    }
}
