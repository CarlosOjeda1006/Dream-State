using System.Collections;
using UnityEngine;

public class FenceController : MonoBehaviour, IInteractable
{
    bool isOpen = false;
    bool opened = false;

    public bool CanInteract => !opened;

    private Animator animator;
    private BoxCollider boxCollider;

    public GameObject gateLock;

    public static string instructionsText;
    public static bool uiActive;
    [SerializeField] GameObject instructionsBox;
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Interact()
    {
        if (isOpen) return;

        if (BoltCuttersController.hasBoltCutters)
        {
            OpenGate();
        }
        else
        {
            PlayLockedFeedback();
        }
    }

    void OpenGate()
    {
        gateLock.SetActive(false);

        animator.SetBool("isOpen", true);
        boxCollider.enabled = false;

        SoundEffectManager.Play("OpenDoor");

        isOpen = true;
        opened = true;
    }

    void PlayLockedFeedback()
    {
        SoundEffectManager.Play("LockedDoor");
        SoundEffectManager.Play("MissingSomething");
        StartCoroutine(ShowMessage());
    }

    IEnumerator ShowMessage()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMPro.TMP_Text>().text = "I'm missing something.";

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);
    }
}