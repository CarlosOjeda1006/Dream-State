using UnityEngine;

public class OpenKeypad : MonoBehaviour
{
    public GameObject keypadOB;

    public bool isOpen;
    private bool canOpen;

    public GameObject hud;
    public GameObject player;

    void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            keypadOB.SetActive(true);

            hud.SetActive(false);

            player.GetComponent<FirstPersonController>().enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            isOpen = true;
        }
    }

    void OnMouseOver()
    {
        if (!isOpen && PlayerCasting.distanceFromTarget < 5)
        {
            canOpen = true;

            UIController.actionText = "Open Safe";
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
        UIController.actionText = "";
        UIController.commandText = "";
        UIController.uiActive = false;

        canOpen = false;
    }
}