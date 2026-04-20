using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged?.Invoke(_health);

            if (_health == 0)
            {
                Die();
            }
        }
    }

    [SerializeField] private int maxHealth = 3;
    public event Action<int> OnHealthChanged;

    private PlayerAnimation anim;

    private void Awake()
    {
        anim = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        Health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        OnHealthChanged?.Invoke(Health);
    }

    private void Die()
    {
        PlayerController_New controller = GetComponent<PlayerController_New>();
        controller.SetCanMove(false);

        anim.SetDie();
        StartCoroutine(DieSequence());
    }

    private IEnumerator DieSequence()
    {
        float destroyDelay = 3f;

        yield return new WaitForSeconds(destroyDelay);

        GameEvents.OnDie?.Invoke();
        Destroy(gameObject);

        //Show GameOver UI
    }
}
