using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu_Scenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Level_One()
    {
        SceneManager.LoadScene("Level_01");
    }

    public void ExitGame()
    {
        Debug.Log("Saliste del juego");
        Application.Quit();
    }
}

