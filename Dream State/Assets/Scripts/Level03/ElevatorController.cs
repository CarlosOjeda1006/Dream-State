using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour, IInteractable
{
    public InventoryController inventory;
    private Animator animator;
    private BoxCollider boxCollider;

    public static string instructionsText;
    public static bool uiActive;
    [SerializeField] GameObject instructionsBox;

    public bool CanInteract => true;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Interact()
    {
        if (!inventory.HasItem("Elevator Card"))
        {

            SoundEffectManager.Play("LockedDoor");
            PlayLockedFeedback();

            return;
        }

        animator.SetBool("isOpen", true);
        boxCollider.enabled = false;

        SoundEffectManager.Play("ElevatorOpen");
        StartCoroutine(EnterElevatorSequence());
    }

    IEnumerator EnterElevatorSequence()
    {

        yield return new WaitForSeconds(2f);
        SoundEffectManager.Play("ElevatorGoDown");

        Invoke("LoadNextDream", 2f);

    }
    void LoadNextDream()
    {
        SceneManager.LoadScene("PruebasShooter");
    }

    void PlayLockedFeedback()
    {
        SoundEffectManager.Play("LockedDoor");
        SoundEffectManager.Play("MissingKeyCard");
        StartCoroutine(ShowMessage());
    }
    IEnumerator ShowMessage()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMPro.TMP_Text>().text = "I'm missing the keycard.";

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);
    }
}