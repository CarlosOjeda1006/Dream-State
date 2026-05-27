using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour, IInteractable
{
    bool isOpen = false;
    bool opened = false;

    public bool CanInteract => !opened;

    private Animator animator;

    public FadeController fadeController;


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

        animator.SetBool("isOpen", true);
        SoundEffectManager.Play("OpenDoor");

        isOpen = true;
        opened = true;
        //LoadNextDream();
        fadeController.StartFade("Credits");
    }

    void LoadNextDream()
    {
        SceneManager.LoadScene("Credits");
    }
}
