using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    bool canOpen;
    bool isOpen = false;
    bool opened = false;
    public bool CanInteract => !opened;

    private Animator animator;

    bool isCorrectDoor;
    public DoorColor doorColor;
    public DoorDir doorDir;

    public Light directionalLight;
    public float transitionDuration = 3f;

    bool nightmareTriggered = false;

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

    public void Interact()
    {
        if (isOpen) return;
        else
        {
            OpenDoor();
        }

    }
    void OpenDoor()
    {

        // Wrong door
        if (!isCorrectDoor)
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

            return;
        }

        // Correct door 
        if (inventory.HasRequiredDreamItems() && isCorrectDoor)
        {
            Debug.Log("All dream items correct");

            animator.SetBool("isOpen", true);

            SoundEffectManager.Play("OpenDoor");

            isOpen = true;
            opened = true;

            Invoke("LoadNextDream", 2f);
        }
        else
        {
            Debug.Log("Opal does not have the right items");

            SoundEffectManager.Play("LockedDoor");

            StartCoroutine(ShowMissingItemsMessage());
        }
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


        ojo1.SetActive(true);
        ojo2.SetActive(true);
        ojo3.SetActive(true);

    }

    IEnumerator ShowMissingItemsMessage()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMPro.TMP_Text>().text = "I'm missing something.";
        SoundEffectManager.Play("MissingSomething");

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);
    }
}