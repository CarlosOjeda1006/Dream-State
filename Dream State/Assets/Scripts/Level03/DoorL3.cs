using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DoorsL2;

public class DoorL3 : MonoBehaviour, IInteractable
{
    bool canOpen;
    bool isOpen = false;
    bool opened = false;
    public bool CanInteract => !opened;

    public static event Action OnJumpscareTriggered;

    public bool jumpscare = false;
    public GameObject jumpscareObject;

    private Animator animator;

    bool isCorrectDoor;
    public DoorNumber doorNumber;
    public GameObject elevatorKeyCard;

    static bool nightmareTriggered = false;

    public InventoryController inventory;

    public static string instructionsText;
    public static bool uiActive;
    [SerializeField] GameObject instructionsBox;
    public enum DoorNumber
    {
        ochocincouno,
        nueveochocuatro,
        seisochodos,
        sietetresocho,
        nuevesietecinco,
        seisseircuatro,
        seisseiscinco,
        nuevenueveseis,
        seisunocuatro,
        dosceroseis
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
        if (inventory.HasRequiredDreamItems() && isCorrectDoor)
        {
            Debug.Log("All dream items correct");

            animator.SetBool("isOpen", true);

            SoundEffectManager.Play("OpenDoor");

            isOpen = true;
            opened = true;

            if (elevatorKeyCard != null)
            {
                elevatorKeyCard.SetActive(true);
            }

            //Invoke("LoadNextDream", 2f);
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
        Debug.Log("CorrectDoorChosen");
    }

    void NightmareEffect()
    {
        Debug.Log("Nightmare triggered");

    }

    IEnumerator ShowMissingItemsMessage()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMPro.TMP_Text>().text = "I'm missing objects.";

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
}

