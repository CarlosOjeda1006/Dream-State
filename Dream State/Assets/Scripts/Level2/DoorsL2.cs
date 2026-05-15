using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorsL2 : MonoBehaviour, IInteractable
{
    bool canOpen;
    [SerializeField] public Keypad keypadLogic;
    bool isOpen = false;
    bool opened = false;
    public bool CanInteract => !opened;

    public static event Action OnJumpscareTriggered;

    public bool jumpscare = false;
    public GameObject jumpscareObject;

    private Animator animator;

    bool isCorrectDoor;
    public DoorSymbol doorSymbol;

    static bool nightmareTriggered = false;

    public InventoryController inventory;

    public static string instructionsText;
    public static bool uiActive;
    [SerializeField] GameObject instructionsBox;

    public enum DoorSymbol
    {
        Spider,
        Snake,
        Eye,
        Moth,
        Stars,
        Moon,
        Cat,
        Dragon,
        Hand,
        Sun
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
        // Door system locked
        if (!keypadLogic.canOpenDoors)
        {
            StartCoroutine(ShowMessage());
            return;
        }

        // Wrong door
        if (!isCorrectDoor)
        {
            animator.SetBool("isOpen", true);

            SoundEffectManager.Play("OpenDoor");

            isOpen = true;
            opened = true;
            
            if (jumpscare)
            {
                JumpscareEffect();
            }
            
            if (!nightmareTriggered)
            {
                nightmareTriggered = true;
                NightmareEffect();
            }

            return;
        }

        // Correct door
        if (inventory.HasRequiredDreamItems())
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
        SceneManager.LoadScene("Level_03");
    }

    void NightmareEffect()
    {
        Debug.Log("Nightmare triggered");

    }

    IEnumerator ShowMissingItemsMessage()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMPro.TMP_Text>().text = "Me faltan objetos.";

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);
    }

    
    void JumpscareEffect()
    {
        jumpscare = false;
        if (jumpscareObject != null)
        {
            SoundEffectManager.Play("Jumpscare");
            jumpscareObject.SetActive(true);
        }
        OnJumpscareTriggered?.Invoke();
    }
    
    IEnumerator ShowMessage()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMPro.TMP_Text>().text = "Need to do something first.";
        SoundEffectManager.Play("SomethingFirst");

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);
    }
}
