using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private List<Image> healthUI = new();

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
        Debug.Log("player has died");
    }
}
