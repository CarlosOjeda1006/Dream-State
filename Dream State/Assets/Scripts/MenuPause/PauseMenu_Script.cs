using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu_Script : MonoBehaviour
{
    public GameObject menuPausa;
    public static bool isPaused;
    private bool enPausa = false;

    public GameObject firstButton;

    void Start()
    {
        menuPausa.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            SoundEffectManager.Play("Pause");

            if (enPausa)
                Reanudar();
            else
                Pausar();
        }
    }

    public void Reanudar()
    {
        //Debug.Log("REANUDAR");
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        enPausa = false;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pausar()
    {
        //Debug.Log("PAUSAR");
        menuPausa.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButton);

        Time.timeScale = 0f;
        enPausa = true;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}