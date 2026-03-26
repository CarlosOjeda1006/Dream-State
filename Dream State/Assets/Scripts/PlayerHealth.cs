using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action<float> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action OnDeath;

    public float maxHealth = 100f;
    public float currentHealth;

    bool isDead;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        damage = Mathf.Max(damage, 0f);

        currentHealth -= damage;

        OnHealthChanged?.Invoke(currentHealth);
        OnDamageTaken?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0f;
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
}