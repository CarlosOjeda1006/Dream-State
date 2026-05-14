using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        Debug.Log(currentPoint);
        transform.position = currentPoint.transform.position;
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

    void HandlePhotoTaken(RaycastHit hit)
    {
        if (currentState != BossState.WaitingForPhoto)
        {
            Debug.Log("primer stop");
            return;
        }

        if (hit.collider == null)
        {
            Debug.Log("Hit collider null");
            return;
        }

        BossPhotoTarget target =
            hit.collider.GetComponent<BossPhotoTarget>();

        if (target == null)
        {
            Debug.Log("Target null");
            return;
        }

        Debug.Log("Nada");
        Debug.Log(hit.collider.transform.position);
        Debug.Log(currentPoint.transform.position);


        if (hit.collider.transform.position == currentPoint.transform.position)
        {
            Debug.Log("Succesful");
            SuccessfulPhoto();
        }
    }

    void SuccessfulPhoto()
    {
        Debug.Log("Succesfully conectido");
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