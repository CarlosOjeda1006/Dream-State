using UnityEngine;
using UnityEngine.EventSystems;

public class OpenKeypad : MonoBehaviour, IInteractable
{
    public GameObject keypadOB;

    public bool isOpen;
    public bool isSolved = false;

    public bool CanInteract => !isSolved;

    public GameObject player;

    public GameObject firstButton;

    public void Interact()
    {
        if (!CanInteract) return;

        if (!isOpen)
            OpenKeyPad();
    }

    void OpenKeyPad()
    {
        keypadOB.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButton);


        player.GetComponent<FirstPersonController>().enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isOpen = true;
    }
}