using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action<float> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action OnDeath;

    public float maxHealth = 100f;
    public float currentHealth;

    //public GameObject healthBarUI;

    public GameObject deathUI;
    bool isDead;

    //public HealthBar healthBar;
    TakeDamage damageFX;


    void Awake()
    {
        currentHealth = maxHealth;
        //healthBar.SetHealth(currentHealth);

        deathUI.SetActive(false);
    }
    void Start()
    {
        damageFX = GetComponentInChildren<TakeDamage>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        damage = Mathf.Max(damage, 0f);
        damageFX.TriggerDamageEffect();

        currentHealth -= damage;
        //healthBar.SetHealth(currentHealth);

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        deathUI.SetActive(true);
        //healthBarUI.SetActive(false);
    }
}