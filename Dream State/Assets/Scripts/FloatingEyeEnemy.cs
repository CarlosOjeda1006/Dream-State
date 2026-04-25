using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FloatingEyeEnemy : MonoBehaviour, IStunneable
{
    Animator anim;

    // Icons
    public Image alertIcon;
    public Image susIcon;

    public Transform target;

    public float detectionRange = 10f;
    public float attackRange = 1.8f;
    public float susRange = 20f;
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

    // Memory
    Vector3 lastKnownPosition;
    bool hasLastKnownPosition = false;

    public float memoryDuration = 5f;
    float memoryTimer = 0f;

    // Scan
    public float scanAngle = 60f;
    public float scanSpeed = 2f;
    float scanTimer = 0f;

    // Flashlight
    public float flashlightDetectionMultiplier = 1.5f;
    LinternaController playerFlashlight;

    AudioSource audioSource;

    protected virtual void Start()
    {
        susIcon.gameObject.SetActive(false);
        alertIcon.gameObject.SetActive(false);

        anim = GetComponentInChildren<Animator>();
        

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
        {
            damageable = target.GetComponent<IDamageable>();
            playerFlashlight = target.GetComponent<LinternaController>();
        }
    }

    protected virtual void Update()
    {
        Hover();

        // STUN
        if (isStunned)
        {
            if (anim != null)
                anim.SetBool("isStunned", true);

            if (Time.time >= stunTime)
            {
                isStunned = false;

                if (anim != null)
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

        float currentDetectionRange = detectionRange;

        // Flashlight effect
        if (playerFlashlight != null && playerFlashlight.isOn)
            currentDetectionRange *= flashlightDetectionMultiplier;

        float currentDetectionRangeSqr = currentDetectionRange * currentDetectionRange;
        float susRangeSqr = susRange * susRange;

        bool inCloseRange = distanceSqr <= currentDetectionRangeSqr;
        bool inVision = CanSeePlayer();

        // DETECT
        if (!hasDetectedPlayer && (inCloseRange || inVision))
        {
            hasDetectedPlayer = true;
            scanTimer = 0f;

            lastKnownPosition = target.position;
            hasLastKnownPosition = true;
            memoryTimer = memoryDuration;

            SoundEffectManager.Play("PlayerDetected");

            susIcon.gameObject.SetActive(false);
            alertIcon.gameObject.SetActive(true);
        }

        // UPDATE MEMORY
        if (hasDetectedPlayer && CanSeePlayer())
        {
            lastKnownPosition = target.position;
            memoryTimer = memoryDuration;
        }

        // SUSPICIOUS
        else if (!hasDetectedPlayer && distanceSqr <= susRangeSqr && CanSeePlayer())
        {
            susIcon.gameObject.SetActive(true);
            alertIcon.gameObject.SetActive(false);
        }

        // IDLE
        else if (!hasDetectedPlayer && !hasLastKnownPosition)
        {
            susIcon.gameObject.SetActive(false);
            alertIcon.gameObject.SetActive(false);
        }

        // LOSE PLAYER
        if (hasDetectedPlayer && (!CanSeePlayer() || distanceSqr > loseTargetDistanceSqr))
        {
            hasDetectedPlayer = false;
        }

        // MEMORY TIMER
        if (!hasDetectedPlayer && hasLastKnownPosition)
        {
            memoryTimer -= Time.deltaTime;

            if (memoryTimer <= 0f)
            {
                hasLastKnownPosition = false;

                alertIcon.gameObject.SetActive(false);
                susIcon.gameObject.SetActive(false);
            }
        }

        // FSM
        if (hasDetectedPlayer && chasePlayer)
        {
            ChasePlayer(distanceSqr);
        }
        else if (hasLastKnownPosition)
        {
            SearchLastPosition();
        }
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
        if (agent == null) return;

        if (distanceToPlayer > attackRangeSqr)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            agent.isStopped = true;
        }

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }
    }

    bool CanSeePlayer()
    {
        if (target == null) return false;

        Vector3 dir = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);

        if (angle > viewAngle / 2f)
            return false;

        float maxVisionDistance = 25f;

        return (target.position - transform.position).sqrMagnitude <= maxVisionDistance * maxVisionDistance;
    }

    void LookForward()
    {
        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }

    protected virtual void TryAttack(float distanceToPlayer)
    {
        if (!hasDetectedPlayer || damageable == null)
            return;

        if (distanceSqr <= attackRangeSqr && Time.time >= nextDamageTime)
        {
            nextDamageTime = Time.time + damageCooldown;
            damageable.TakeDamage(damage);
        }
    }

    void SearchLastPosition()
    {
        if (agent == null) return;

        agent.isStopped = false;
        agent.SetDestination(lastKnownPosition);

        Vector3 dir = lastKnownPosition - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            agent.isStopped = true;

            susIcon.gameObject.SetActive(true);
            alertIcon.gameObject.SetActive(false);

            ScanAround();
        }
    }

    void ScanAround()
    {
        scanSpeed = Mathf.Lerp(2f, 0.8f, 1f - (memoryTimer / memoryDuration));
        scanTimer += Time.deltaTime * scanSpeed;

        float angle = Mathf.Sin(scanTimer) * scanAngle;

        Vector3 dir = lastKnownPosition - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f)
            dir = transform.forward;

        Quaternion baseRot = Quaternion.LookRotation(dir);
        Quaternion scanRot = Quaternion.Euler(0f, angle, 0f);

        transform.rotation = Quaternion.Slerp(transform.rotation, baseRot * scanRot, rotationSpeed * Time.deltaTime);
    }

    public void Stun(float duration)
    {
        SoundEffectManager.Play("StunFlash");

        isStunned = true;
        stunTime = Time.time + duration;

        if (agent != null)
            agent.isStopped = true;
    }
}