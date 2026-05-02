using TMPro;
using UnityEngine;
using System.Collections;

public class OpalLines : MonoBehaviour
{
    [SerializeField] GameObject instructionsBox;

    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hasPlayed != true)
        {
            StartCoroutine(Message());
        }
    }

    IEnumerator Message()
    {

        instructionsBox.SetActive(true);
        TMP_Text text = instructionsBox.GetComponent<TMP_Text>();

        text.text = "That drawing, it must mean something.";
        SoundEffectManager.Play("MeanSomething");

        yield return new WaitForSeconds(2.5f);

        instructionsBox.SetActive(false);


        hasPlayed = true;
    }
}
