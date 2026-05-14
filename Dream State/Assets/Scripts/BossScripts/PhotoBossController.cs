using System.Collections;
using UnityEngine;

public class PhotoBossController : MonoBehaviour
{
    public enum BossState
    {
        Idle,
        WaitingForPhoto,
        Dead
    }

    [Header("References")]
    public PlayerCameraFlash playerCamera;
    public Transform playerCameraTransform;

    [Header("Directions")]
    public BossDirectionPoint[] points;

    [Header("Gameplay")]
    public float responseTime = 3f;
    public int successRequired = 5;

    int currentSuccesses;

    BossDirectionPoint currentPoint;

    Coroutine currentRoutine;

    BossState currentState;

    void OnEnable()
    {
        playerCamera.OnPhotoTaken += HandlePhotoTaken;
    }

    void OnDisable()
    {
        playerCamera.OnPhotoTaken -= HandlePhotoTaken;
    }

    void Start()
    {
        StartEncounter();
    }

    void StartEncounter()
    {
        PickNextDirection();
    }

    void PickNextDirection()
    {
        currentState = BossState.WaitingForPhoto;

        currentPoint =
            points[Random.Range(0, points.Length)];

        // AQUI:
        // reproducir sonido direccional
        // activar whisper
        // etc

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine =
            StartCoroutine(ResponseTimer());
    }

    IEnumerator ResponseTimer()
    {
        yield return new WaitForSeconds(responseTime);

        KillPlayer();
    }

    void HandlePhotoTaken()
    {
        if (currentState != BossState.WaitingForPhoto)
            return;

        if (PlayerIsLookingCorrectDirection())
        {
            SuccessfulPhoto();
        }
    }

    bool PlayerIsLookingCorrectDirection()
    {
        Vector3 dirToPoint =
            (currentPoint.transform.position
            - playerCameraTransform.position).normalized;

        float dot =
            Vector3.Dot(
                playerCameraTransform.forward,
                dirToPoint
            );

        return dot >= 0.92f;
    }

    void SuccessfulPhoto()
    {
        StopCoroutine(currentRoutine);

        currentSuccesses++;

        // FLASH ENEMY SCREAM TELEPORT EFFECT

        if (currentSuccesses >= successRequired)
        {
            WinEncounter();

            return;
        }

        PickNextDirection();
    }

    void KillPlayer()
    {
        currentState = BossState.Dead;

        Debug.Log("PLAYER DEAD");

        IDamageable damageable =
            PlayerSingle.instance.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(9999f);
        }
    }

    void WinEncounter()
    {
        currentState = BossState.Idle;

        Debug.Log("BOSS DEFEATED");

        // cinematic unlock door etc
    }
}