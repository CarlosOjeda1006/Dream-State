using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIController : MonoBehaviour
{
    public void MainMenu()
    {
        Debug.Log("go to Main menu");
        SceneManager.LoadScene("MainMenu");
    }
    public void Retry()
    {
        Debug.Log("go to Main menu");
        SceneManager.LoadScene("Level_01");
    }
}
