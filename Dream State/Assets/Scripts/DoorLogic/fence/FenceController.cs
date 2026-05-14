using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FenceController : MonoBehaviour
{
    bool canOpen;
    bool isOpen = false;

    private Animator animator;

    public GameObject boltCutters;
    public GameObject gateLock;
    private BoxCollider boxCollider;

    public static string instructionsText;
    public static bool uiActive;
    [SerializeField] GameObject instructionsBox;


    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        // OPEN
        if (canOpen && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {

            if (BoltCuttersController.hasBoltCutters)
            {
                gateLock.SetActive(false);
                animator.SetBool("isOpen", true);
                boxCollider.enabled = false;

                SoundEffectManager.Play("OpenDoor");
                isOpen = true;
            }
            else
            {
                Debug.Log("Does not have BoltCutters to open");

                SoundEffectManager.Play("LockedDoor");

                StartCoroutine(ShowMissingItemsMessage());
            }
        }
    }

    void OnMouseOver()
    {
        if (!isOpen && PlayerCasting.distanceFromTarget < 5)
        {
            canOpen = true;
            UIController.actionText = "Open Gate";
            UIController.commandText = "Open";
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


    IEnumerator ShowMissingItemsMessage()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMPro.TMP_Text>().text = "I'm missing something.";
        SoundEffectManager.Play("MissingSomething");

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);
    }
}
