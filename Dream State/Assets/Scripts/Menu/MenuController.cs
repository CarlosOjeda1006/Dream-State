using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public static bool isMenuOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            bool isActive = !menuCanvas.activeSelf;
            SoundEffectManager.Play("Inventory_Open");
            menuCanvas.SetActive(isActive);
            isMenuOpen = isActive;

            //  Control del cursor
            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isActive;
        }
    }
}
