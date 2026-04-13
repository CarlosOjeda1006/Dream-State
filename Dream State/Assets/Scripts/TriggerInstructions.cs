using System.Collections;
using UnityEngine;
using TMPro;

public class TriggerInstructions : MonoBehaviour
{
    [SerializeField] GameObject instructionsBox;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ToofarMessage());
        }
    }

    IEnumerator ToofarMessage()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMP_Text>().text = "No puedo alejarme del origen.";

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);
    }
}