using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    bool canOpen;
    bool isOpen = false;

    private Animator animator;

    bool isCorrectDoor;
    public DoorColor doorColor;
    public DoorDir doorDir;

    public Light directionalLight;
    public float transitionDuration = 3f;

    private Color nightColor = new Color(0.2f, 0.3f, 0.6f);
    private float nightIntensity = 0.1f;
    static bool nightmareTriggered = false;

    public InventoryController inventory;

    public GameObject ojo1;
    public GameObject ojo2;
    public GameObject ojo3;

    public static string instructionsText;
    public static bool uiActive;
    [SerializeField] GameObject instructionsBox;

    public enum DoorColor
    {
        Red,
        Blue,
        Yellow,
        Green,
        Purple,
        Orange
    }

    public enum DoorDir
    {
        Right,
        Left
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.E) && !isOpen && !isCorrectDoor)
        {
            animator.SetBool("isOpen", true);
            SoundEffectManager.Play("OpenDoor");
            SoundEffectManager.Play("NightmareTrigger");
            isOpen = true;

            if (!nightmareTriggered)
            {
                nightmareTriggered = true;
                NightmareEffect();
            }
        }

        
        // OPEN
        if (canOpen && Input.GetKeyDown(KeyCode.E) && !isOpen && isCorrectDoor)
        {

            if (inventory.HasRequiredDreamItems())
            {
                Debug.Log("All dream items correct");
                animator.SetBool("isOpen", true);
                SoundEffectManager.Play("OpenDoor");
                isOpen = true;
                Invoke("LoadNextDream", 2f);
            }
            else
            {
                Debug.Log("Opal does not have the right items or the wrong ones");

                SoundEffectManager.Play("LockedDoor");

                StartCoroutine(ShowMissingItemsMessage());
            }
        }
       

        // CLOSE
        /*
        if (isOpen && PlayerCasting.distanceFromTarget > 5)
        {
            animator.SetBool("isOpen", false);
            SoundEffectManager.Play("CloseDoor");
            isOpen = false;
        }
        */
    }

    void OnMouseOver()
    {
        if (!isOpen && PlayerCasting.distanceFromTarget < 5)
        {
            canOpen = true;
            UIController.actionText = "Abrir Puerta";
            UIController.commandText = "Abrir";
            UIController.uiActive = true;
        }
        else
        {
            ResetUI();
        }
    }

    void OnMouseExit()
    {
        ResetUI();
    }

    void ResetUI()
    {
        canOpen = false;
        UIController.actionText = "";
        UIController.commandText = "";
        UIController.uiActive = false;
    }

    public void SetCorrect(bool value)
    {
        isCorrectDoor = value;

    }

    void LoadNextDream()
    {
        SceneManager.LoadScene("Level_02");
    }

    void NightmareEffect()
    {
        Debug.Log("Nightmare triggered");

        if (directionalLight != null)
        {
            StartCoroutine(DayToNight());
        }

        ojo1.SetActive(true);
        ojo2.SetActive(true);
        ojo3.SetActive(true);

    }

    IEnumerator DayToNight()
    {
        Color startColor = directionalLight.color;
        float startIntensity = directionalLight.intensity;
        
        float time = 0;

        while (time < transitionDuration)
        {
            time += Time.deltaTime;

            float t = time / transitionDuration;

            directionalLight.color = Color.Lerp(startColor, nightColor, t);
            directionalLight.intensity = Mathf.Lerp(startIntensity, nightIntensity, t);

            RenderSettings.ambientIntensity = Mathf.Lerp(1f, 0.2f, t);

            yield return null;
        }
    }

    IEnumerator ShowMissingItemsMessage()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMPro.TMP_Text>().text = "Me faltan objetos.";

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);
    }
}