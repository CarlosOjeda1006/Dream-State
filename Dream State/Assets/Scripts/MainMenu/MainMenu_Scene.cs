using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu_Scene : MonoBehaviour
{
    public void LoadLevelOne()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Saliste del juego");
    }
}