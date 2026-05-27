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

    [Header("Boss Audio")]
    public BossAudioSystem bossAudioSystem;
    int currentSuccesses;

    [Header("Police tape")]
    public GameObject policeTape;
    public GameObject multiplePoliceTapes;
    public GameObject camera;
    public GameObject light;
    public GameObject mainLight;
    public GameObject finalDoor;
    [Header("Visuals")]
    public GameObject otherVisualhead;
    public GameObject otherVisualhair;
    public GameObject otherVisualbody;


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
        otherVisualbody.SetActive(false);
        otherVisualhead.SetActive(false);
        otherVisualhair.SetActive(false);
    }

    void StartEncounter()
    {
        Debug.Log("Encounter started");
        PickNextDirection();
    }

    void PickNextDirection()
    {
        currentState = BossState.WaitingForPhoto;
        
        currentPoint =
            points[Random.Range(0, points.Length)];
        Debug.Log(currentPoint);
        transform.position = currentPoint.transform.position;

        RotateTowardsPlayer();

        Debug.Log("Playing direction sound");
        bossAudioSystem.PlayDirectionSound(currentPoint.direction);

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

        policeTape.SetActive(false);
        camera.SetActive(false);
        light.SetActive(true);
        finalDoor.SetActive(true);

        otherVisualbody.SetActive(false);
        otherVisualhead.SetActive(false);
        otherVisualhair.SetActive(false);
    }

    void RotateTowardsPlayer()
    {
        if (playerCameraTransform == null)
            return;

        Vector3 dir =
            playerCameraTransform.position - transform.position;

        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        transform.rotation = Quaternion.LookRotation(dir);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(BeginEncounterSequence());

            GetComponent<Collider>().enabled = false;
        }
    }
    IEnumerator BeginEncounterSequence()
    {
        mainLight.SetActive(false);
        InstructionsUI.Instance.ShowInstruction(
            "<color=#B84848><b>You can't let her touch you. You must capture The Other inside a memory. Press RB to take a picture.</b></color>"
        );
        policeTape.SetActive(true);
        multiplePoliceTapes.SetActive(true);
        otherVisualbody.SetActive(true);
        otherVisualhead.SetActive(true);
        otherVisualhair.SetActive(true);

        yield return new WaitForSeconds(3f);

        StartEncounter();
    }
}