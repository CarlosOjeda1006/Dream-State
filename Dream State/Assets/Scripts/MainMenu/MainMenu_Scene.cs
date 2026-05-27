using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MainMenu_Scene : MonoBehaviour
{
    public GameObject firstButton;
    

    public void LoadLevelOne()
    {
        Load.SceneToLoad = "Level_01";
        SceneManager.LoadScene("Loading");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Saliste del juego");
    }
}