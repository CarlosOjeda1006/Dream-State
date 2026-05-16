using System.Collections;
using UnityEngine;

public class ShooterCombat : MonoBehaviour
{
    [Header("References")]
    public Transform target;

    [Header("Aim")]
    public float aimDuration = 2.5f;
    public float rotationSpeed = 8f;

    [Header("Charge")]
    public float delayBeforeCharge = 1.5f;

    [Header("Flash")]
    public GameObject gunFlash;
    public float flashDuration = 0.05f;

    [Header("VFX")]
    TakeDamage damageFX;

    bool isAttacking;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        damageFX = GetComponentInChildren<TakeDamage>();
    }
    public IEnumerator AttackSequence(System.Action onCharge)
    {
        if (isAttacking)
            yield break;

        isAttacking = true;

        // AQUI VA LA MUSICA CREEPY

        float timer = 0f;

        while (timer < aimDuration)
        {
            timer += Time.deltaTime;

            RotateTowardsTarget();

            yield return null;
        }

        Shoot();

        yield return new WaitForSeconds(delayBeforeCharge);

        onCharge?.Invoke();
    }

    void RotateTowardsTarget()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;

        dir.y = 0f;

        if (dir.sqrMagnitude <= 0.001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }

    void Shoot()
    {
        animator.SetBool("detectedPlayer", true);
        Debug.Log("BANG");
        animator.SetTrigger("Shoot");

        // PLAYER RECIBE SLOW
        PlayerStatus status = target.GetComponent<PlayerStatus>();


        if (status != null)
        {
            status.ApplySlow(0.2f, 6f);
        }

        // VFX
        damageFX.TriggerDamageEffect();
        SoundEffectManager.Play("GunShot");
        // FLASH
        StartCoroutine(GunFlash());
    }
    IEnumerator GunFlash()
    {
        yield return new WaitForSeconds(0.5f);
        gunFlash.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        gunFlash.SetActive(false);
    }
}