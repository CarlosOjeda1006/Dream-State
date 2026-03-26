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

    float detectionRangeSqr;
    float attackRangeSqr;
    float loseTargetDistanceSqr;
    float distanceSqr;

    Vector3 startPosition;
    IDamageable damageable;
    float nextDamageTime;
    bool hasDetectedPlayer;

    void Start()
    {
        detectionRangeSqr = detectionRange * detectionRange;
        attackRangeSqr = attackRange * attackRange;
        loseTargetDistanceSqr = loseTargetDistance * loseTargetDistance;

        startPosition = transform.position;

        if (target == null)
        {
            GameObject playerObject = PlayerSingle.instance.gameObject;

            if (playerObject != null)
                target = playerObject.transform;
        }

        if (target != null)
            damageable = target.GetComponent<IDamageable>();
        

    }

    void Update()
    {
        Hover();

        if (target == null)
            return;

        Vector3 offset = target.position - transform.position;
        distanceSqr = offset.sqrMagnitude;

        if (!hasDetectedPlayer && distanceSqr <= detectionRangeSqr)
            hasDetectedPlayer = true;

        if (hasDetectedPlayer && distanceSqr > loseTargetDistanceSqr)
            hasDetectedPlayer = false;

        if (hasDetectedPlayer && chasePlayer)
            ChasePlayer(distanceSqr);
        else
            LookForward();

        Debug.Log("Quiero atacar");
        Debug.Log(hasDetectedPlayer);
        TryAttack(distanceSqr);
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

        if (distanceToPlayer > attackRangeSqr)
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
        if (!hasDetectedPlayer || damageable == null)
            return;
        //distanceSqr <= attackRangeSqr &&
        if ( Time.time >= nextDamageTime)
        {
            nextDamageTime = Time.time + damageCooldown;
            damageable?.TakeDamage(damage);
            Debug.Log("Estoy intentando atacar");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRangeSqr);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRangeSqr);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, loseTargetDistanceSqr);
    }
}