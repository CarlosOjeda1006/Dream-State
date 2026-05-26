using TMPro;
using UnityEngine;
using System.Collections;

public class OpalLines : MonoBehaviour
{
    [SerializeField] GameObject instructionsBox;

    [TextArea]
    public string line;

    public string soundName;

    private bool hasPlayed = false;
    public bool shouldDissapear;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            StartCoroutine(Message());
        }
    }

    IEnumerator Message()
    {
        instructionsBox.SetActive(true);

        TMP_Text text = instructionsBox.GetComponent<TMP_Text>();

        text.text = line;

        if (!string.IsNullOrEmpty(soundName))
        {
            SoundEffectManager.Play(soundName);
        }

        yield return new WaitForSeconds(2.5f);

        instructionsBox.SetActive(false);

        hasPlayed = true;

        if (shouldDissapear) gameObject.SetActive(false);
    }
}