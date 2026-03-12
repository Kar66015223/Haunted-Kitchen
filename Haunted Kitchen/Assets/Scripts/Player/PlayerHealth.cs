using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    private int _health;
    public int health
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
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnHealthChanged?.Invoke(health);
    }

    private void Die()
    {
        anim.SetDie();
        StartCoroutine(DieSequence());

        Debug.Log("player has died");
    }

    private IEnumerator DieSequence()
    {
        float destroyDelay = 3f;

        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);

        //Show GameOver UI
    }
}
