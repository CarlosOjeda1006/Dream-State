using UnityEngine;

public class BossAudioSystem : MonoBehaviour
{
    public Animator animator;

    public enum BossState
    {
        Passive,
        Mid,
        Crazy
    }

    public BossState currentState;

    [Header("North")]
    public AudioClip[] northPassive;
    public AudioClip[] northMid;
    public AudioClip[] northCrazy;

    [Header("South")]
    public AudioClip[] southPassive;
    public AudioClip[] southMid;
    public AudioClip[] southCrazy;

    [Header("East")]
    public AudioClip[] eastPassive;
    public AudioClip[] eastMid;
    public AudioClip[] eastCrazy;

    [Header("West")]
    public AudioClip[] westPassive;
    public AudioClip[] westMid;
    public AudioClip[] westCrazy;

    [Header("Sources")]
    public AudioSource loopSource;
    public AudioSource oneShotSource;

    AudioClip lastClipPlayed;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayDirectionSound(BossDirectionPoint.Direction dir)
    {
        AudioClip[] clipsToUse = null;

        switch (dir)
        {
            case BossDirectionPoint.Direction.North:
                animator.SetBool("pose1", true);

                clipsToUse = GetClipsByState(
                    northPassive,
                    northMid,
                    northCrazy);

                break;

            case BossDirectionPoint.Direction.South:
                animator.SetBool("pose2", true);

                clipsToUse = GetClipsByState(
                    southPassive,
                    southMid,
                    southCrazy);

                break;

            case BossDirectionPoint.Direction.East:
                animator.SetBool("pose3", true);

                clipsToUse = GetClipsByState(
                    eastPassive,
                    eastMid,
                    eastCrazy);

                break;

            case BossDirectionPoint.Direction.West:
                animator.SetBool("pose4", true);

                clipsToUse = GetClipsByState(
                    westPassive,
                    westMid,
                    westCrazy);

                break;
        }

        PlayRandomClip(clipsToUse);
    }

    AudioClip[] GetClipsByState(
        AudioClip[] passive,
        AudioClip[] mid,
        AudioClip[] crazy)
    {
        switch (currentState)
        {
            case BossState.Passive:
                return passive;

            case BossState.Mid:
                return mid;

            case BossState.Crazy:
                return crazy;
        }

        return passive;
    }

    void PlayRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        AudioClip selectedClip;

        do
        {
            selectedClip = clips[Random.Range(0, clips.Length)];
        }
        while (clips.Length > 1 && selectedClip == lastClipPlayed);

        lastClipPlayed = selectedClip;

        oneShotSource.PlayOneShot(selectedClip);
    }

    public void ResetBools()
    {
        animator.SetBool("pose1", false);
        animator.SetBool("pose2", false);
        animator.SetBool("pose3", false);
        animator.SetBool("pose4", false);
    }
}