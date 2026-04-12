using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action<float> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action OnDeath;

    public float maxHealth = 100f;
    public float currentHealth;

    public GameObject deathUI;
    bool isDead;

    void Awake()
    {
        currentHealth = maxHealth;
        deathUI.SetActive(false);
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
            SoundEffectManager.Play("PlayerDeath");
            currentHealth = 0f;
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        
        isDead = true;
        OnDeath?.Invoke();
        GetComponent<CharacterController>().enabled = false;
        GetComponent<FirstPersonController>().enabled = false;
        deathUI.SetActive(true);
        
    }
}