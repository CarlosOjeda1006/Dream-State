using UnityEngine;

public class ShooterEnemyAudio : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource loopSource;
    public AudioSource oneShotSource;

    [Header("Clips")]
    public AudioClip patrolClip;
    public AudioClip searchingClip;
    public AudioClip detectedClip;
    public AudioClip chargeClip;

    void Start()
    {
        PlayPatrol();
    }

    public void PlayPatrol()
    {
        if (loopSource.clip == patrolClip)
            return;

        loopSource.Stop();
        loopSource.clip = patrolClip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void PlaySearching()
    {
        loopSource.Stop();
        loopSource.clip = searchingClip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void PlayDetected()
    {
        oneShotSource.PlayOneShot(detectedClip);
    }

    public void PlayCharge()
    {
        loopSource.Stop();
        loopSource.clip = chargeClip;
        loopSource.loop = true;
        loopSource.Play();
    }
}