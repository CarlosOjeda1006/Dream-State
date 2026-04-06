using UnityEngine;

public class Door : MonoBehaviour
{
    bool canOpen;
    bool isOpen = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // OPEN
        if (canOpen && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            animator.SetBool("isOpen", true);
            SoundEffectManager.Play("OpenDoor");
            isOpen = true;
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
        if (PlayerCasting.distanceFromTarget < 5)
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
}