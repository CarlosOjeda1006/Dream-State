using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorsL2 : MonoBehaviour
{
    bool canOpen;
    bool isOpen = false;
    bool opened = false;

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
        Sun
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
                Debug.Log("Opal does not have the right items");

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
        if (PlayerCasting.distanceFromTarget < 5 && !opened)
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
        SceneManager.LoadScene("Level_01");
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
            jumpscareObject.SetActive(true);
        }
        OnJumpscareTriggered?.Invoke();
    }
}
