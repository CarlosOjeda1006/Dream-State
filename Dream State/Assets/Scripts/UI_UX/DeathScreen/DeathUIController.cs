using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeathUIController : MonoBehaviour
{
    public GameObject firstButton;
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        StopAllAudio();

        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        StopAllAudio();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void StopAllAudio()
    {
        AudioSource[] allAudio = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource audioSource in allAudio)
        {
            audioSource.Stop();
        }
    }
}