using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorsL2Normal : MonoBehaviour, IInteractable
{
    bool isOpen = false;
    bool opened = false;

    public bool CanInteract => !opened;

    private Animator animator;

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
    }

}
