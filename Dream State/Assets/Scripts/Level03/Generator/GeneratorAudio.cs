using UnityEngine;

public class GeneratorAudio : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource loopSource;
    public AudioSource oneShotSource;

    [Header("Clips")]
    public AudioClip onClip;
    public AudioClip offClip;

    void Start()
    {
        PlayOn();
    }

    public void PlayOn()
    {
        if (loopSource.clip == onClip)
            return;

        loopSource.Stop();
        loopSource.clip = onClip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void PlayOff()
    {
        oneShotSource.PlayOneShot(offClip);
    }
}
