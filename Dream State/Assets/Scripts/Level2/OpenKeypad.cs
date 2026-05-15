using UnityEngine;

public class OpenKeypad : MonoBehaviour, IInteractable
{
    public GameObject keypadOB;

    public bool isOpen;
    public bool isSolved = false;

    public bool CanInteract => !isSolved;

    public GameObject hud;
    public GameObject player;

    public void Interact()
    {
        if (!CanInteract) return;

        if (!isOpen)
            OpenKeyPad();
    }

    void OpenKeyPad()
    {
        keypadOB.SetActive(true);
        hud.SetActive(false);

        player.GetComponent<FirstPersonController>().enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isOpen = true;
    }
}