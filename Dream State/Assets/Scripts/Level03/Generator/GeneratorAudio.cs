using UnityEngine;

public class GeneratorAudio : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource loopSource;
    public AudioSource oneShotSource;

    [Header("Clips")]
    public AudioClip onClip;
    public AudioClip offClip;


    public void PlayOn()
    {
        if (loopSource.isPlaying)
            return;

        loopSource.clip = onClip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void PlayOff()
    {
        loopSource.Stop();
        oneShotSource.PlayOneShot(offClip);
    }
}
