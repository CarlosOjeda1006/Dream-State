using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject healthBar;
    public GameObject noteViewerPanel;

    public GameObject player;

    public static bool isMenuOpen = false;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (noteViewerPanel.activeSelf)
                return;

            bool isActive = !menuCanvas.activeSelf;

            SoundEffectManager.Play("Inventory_Open");

            menuCanvas.SetActive(isActive);
            healthBar.SetActive(!isActive);

            isMenuOpen = isActive;

            // Pause game
            Time.timeScale = isActive ? 0f : 1f;

            player.GetComponent<FirstPersonController>().enabled =
                !isActive;

            Cursor.lockState =
                isActive
                ? CursorLockMode.None
                : CursorLockMode.Locked;

            Cursor.visible = isActive;
        }
    }
}