using UnityEngine;

public class FloatingEyeEnemy : MonoBehaviour
{
    public Transform target;
    public float detectionRange = 10f;
    public float attackRange = 1.8f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 6f;
    public float hoverHeight = 2f;
    public float hoverAmplitude = 0.25f;
    public float hoverFrequency = 2f;
    public float damage = 10f;
    public float damageCooldown = 1f;
    public float loseTargetDistance = 14f;
    public bool chasePlayer = true;

    Vector3 startPosition;
    PlayerHealth playerHealth;
    float nextDamageTime;
    bool hasDetectedPlayer;

    void Start()
    {
        startPosition = transform.position;

        if (target == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                target = playerObject.transform;
        }

        if (target != null)
            playerHealth = target.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        Hover();

        if (target == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (!hasDetectedPlayer && distanceToPlayer <= detectionRange)
            hasDetectedPlayer = true;

        if (hasDetectedPlayer && distanceToPlayer > loseTargetDistance)
            hasDetectedPlayer = false;

        if (hasDetectedPlayer && chasePlayer)
            ChasePlayer(distanceToPlayer);
        else
            LookForward();

        TryAttack(distanceToPlayer);
    }

    void Hover()
    {
        Vector3 position = transform.position;
        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        position.y = startPosition.y + hoverOffset;
        transform.position = position;
    }

    void ChasePlayer(float distanceToPlayer)
    {
        Vector3 targetPosition = target.position;
        targetPosition.y = transform.position.y;

        if (distanceToPlayer > attackRange)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void LookForward()
    {
        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }

    void TryAttack(float distanceToPlayer)
    {
        if (!hasDetectedPlayer || playerHealth == null)
            return;

        if (distanceToPlayer <= attackRange && Time.time >= nextDamageTime)
        {
            nextDamageTime = Time.time + damageCooldown;
            playerHealth.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, loseTargetDistance);
    }
}