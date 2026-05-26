using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action<float> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action OnDeath;

    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Death")]
    public GameObject deathUI;
    public GameObject[] LevelUI; 
    bool isDead;

    TakeDamage damageFX;


    void Awake()
    {
        currentHealth = maxHealth;

        deathUI.SetActive(false);
    }
    void Start()
    {
        BoltCuttersController.hasBoltCutters = false;
        damageFX = GetComponentInChildren<TakeDamage>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        damage = Mathf.Max(damage, 0f);
        damageFX.TriggerDamageEffect();

        currentHealth -= damage;

        OnHealthChanged?.Invoke(currentHealth);
        OnDamageTaken?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            SoundEffectManager.Play("PlayerDeath");
            currentHealth = 0f;
            Die();
        }
        if(currentHealth < 20f)
        {
            SoundEffectManager.Play("Heartbeat");
        }
    }

    void Die()
    {
        if (isDead) return;

        DiffManager.Instance.deaths++;
        isDead = true;
        OnDeath?.Invoke();
        GetComponent<CharacterController>().enabled = false;
        GetComponent<FirstPersonController>().enabled = false;
        foreach (GameObject ui in LevelUI)
        {
            ui.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        deathUI.SetActive(true);
    }
}