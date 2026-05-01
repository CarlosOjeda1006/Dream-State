using System.Collections;
using UnityEngine;
using TMPro;

public class TriggerInstructions : MonoBehaviour
{
    [SerializeField] GameObject instructionsBox;

    public float cooldown = 5f;
    private bool isPlaying = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ToofarMessage());
        }
    }

    IEnumerator ToofarMessage()
    {
        isPlaying = true;

        instructionsBox.SetActive(true);
        TMP_Text text = instructionsBox.GetComponent<TMP_Text>();

        int random = Random.Range(0, 3);

        switch (random)
        {
            case 0:
                text.text = "I can't go too far away.";
                SoundEffectManager.Play("TooFar");
                break;

            case 1:
                text.text = "I can't stray from the origin.";
                SoundEffectManager.Play("StrayOrigin");
                break;

            case 2:
                text.text = "I could get lost. It all looks the same that way.";
                SoundEffectManager.Play("Lost");
                break;
        }

        yield return new WaitForSeconds(2.5f);

        instructionsBox.SetActive(false);

        yield return new WaitForSeconds(cooldown);

        isPlaying = false;
    }
}