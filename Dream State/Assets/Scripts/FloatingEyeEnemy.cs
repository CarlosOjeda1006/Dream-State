using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FloatingEyeEnemy : MonoBehaviour, IStunneable
{

    Animator anim;

    //icons for states
    public Image alertIcon;
    public Image susIcon;

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
    public NavMeshAgent agent;

    public float viewAngle = 180f;

    protected float detectionRangeSqr;
    protected float attackRangeSqr;
    protected float loseTargetDistanceSqr;
    protected float distanceSqr;

    Vector3 startPosition;
    IDamageable damageable;
    protected float nextDamageTime;
    protected bool hasDetectedPlayer;
    float baseOffsetStart;

    protected bool isStunned = false;
    protected float stunTime;


    AudioSource audioSource;

    protected virtual void Start()
    {
        susIcon.gameObject.SetActive(false);
        alertIcon.gameObject.SetActive(false);

        anim = GetComponentInChildren<Animator>();

        audioSource = GetComponent<AudioSource>();

        detectionRangeSqr = detectionRange * detectionRange;
        attackRangeSqr = attackRange * attackRange;
        loseTargetDistanceSqr = loseTargetDistance * loseTargetDistance;

        startPosition = transform.position;

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = attackRange;
            agent.updateRotation = false;
            baseOffsetStart = agent.baseOffset;
        }

        if (target == null)
        {
            GameObject playerObject = PlayerSingle.instance.gameObject;

            if (playerObject != null)
                target = playerObject.transform;
        }

        if (target != null)
            damageable = target.GetComponent<IDamageable>();
    }

    protected virtual void Update()
    {
        Hover();

        if (isStunned)
        {
            anim.SetBool("isStunned", true);

            if (Time.time >= stunTime)
            {
                isStunned = false;
                anim.SetBool("isStunned", false);

                if (agent != null)
                    agent.isStopped = false;
            }
            else
            {
                return;
            }
        }

        if (target == null)
            return;

        Vector3 offset = target.position - transform.position;
        distanceSqr = offset.sqrMagnitude;

        //  DETECTADO
        if (!hasDetectedPlayer && distanceSqr <= detectionRangeSqr && CanSeePlayer())
        {
            hasDetectedPlayer = true;

            SoundEffectManager.Play("PlayerDetected");

            susIcon.gameObject.SetActive(false);
            alertIcon.gameObject.SetActive(true);
        }

        //  SOSPECHA
        else if (!hasDetectedPlayer && distanceSqr <= detectionRangeSqr * 2.5f && CanSeePlayer())
        {
            //SoundEffectManager.Play("Suspicious");
            susIcon.gameObject.SetActive(true);
            alertIcon.gameObject.SetActive(false);
        }

        //  NADA
        else if (!hasDetectedPlayer)
        {
            susIcon.gameObject.SetActive(false);
            alertIcon.gameObject.SetActive(false);
        }

        //  PIERDE AL JUGADOR
        if (hasDetectedPlayer && distanceSqr > loseTargetDistanceSqr)
        {
            hasDetectedPlayer = false;

            alertIcon.gameObject.SetActive(false);
            susIcon.gameObject.SetActive(false);
        }

        if (hasDetectedPlayer && chasePlayer)
            ChasePlayer(distanceSqr);
        else
        {
            if (agent != null)
                agent.isStopped = true;

            LookForward();
        }


        TryAttack(distanceSqr);
    }

 

    void Hover()
    {
        if (agent == null)
            return;

        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        agent.baseOffset = baseOffsetStart + hoverOffset;
    }

    void ChasePlayer(float distanceToPlayer)
    {
        Vector3 targetPosition = target.position;

        if (agent != null)
        {
            agent.speed = moveSpeed;

            if (distanceToPlayer > attackRangeSqr)
            {
                agent.isStopped = false;
                agent.SetDestination(targetPosition);
            }
            else
            {
                agent.isStopped = true;
            }
        }

        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    bool CanSeePlayer()
    {
        if (target == null) return false;

        Vector3 directionToPlayer = (target.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return angle < viewAngle / 2f;
    }

    void LookForward()
    {
        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }
    protected virtual void OnAttack()
    {
    }

    protected virtual void TryAttack(float distanceToPlayer)
    {
        if (!hasDetectedPlayer || damageable == null)
            return;

        if (distanceSqr <= attackRangeSqr && Time.time >= nextDamageTime)
        {
            nextDamageTime = Time.time + damageCooldown;
            damageable?.TakeDamage(damage);
            OnAttack();
            Debug.Log("Base attack triggered");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, loseTargetDistance);

        Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, left * detectionRange);
        Gizmos.DrawRay(transform.position, right * detectionRange);
    }

    public void Stun(float duration)
    {
        SoundEffectManager.Play("StunFlash");
        Debug.Log("STUN TRIGGERED");
        isStunned = true;
        stunTime = Time.time + duration;

        if (agent != null)
        {
            agent.isStopped = true;
            
        }
    }
}