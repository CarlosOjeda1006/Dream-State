using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIController : MonoBehaviour
{
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

        SceneManager.LoadScene("Level_01");
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