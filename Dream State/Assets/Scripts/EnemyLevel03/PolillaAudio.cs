using UnityEngine;

public class PolillaAudio : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource loopSource;

    [Header("Clips")]
    public AudioClip patrolClip;
    public AudioClip attackClip;

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

    public void PlayAttack()
    {
        loopSource.Stop();
        loopSource.clip = attackClip;
        loopSource.loop = true;
        loopSource.Play();
    }
}
