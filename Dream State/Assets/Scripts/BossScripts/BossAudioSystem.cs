using UnityEngine;

public class BossAudioSystem : MonoBehaviour
{
    bool pose1 = false;
    bool pose2 = false;
    bool pose3 = false;
    bool pose4 = false;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayDirectionSound(
        BossDirectionPoint.Direction dir)
    {
        switch (dir)
        {
            case BossDirectionPoint.Direction.North:
                animator.SetBool("pose1", true);
                //Audio norte;
                break;

            case BossDirectionPoint.Direction.South:
                animator.SetBool("pose2", true);
                //Audio sur
                break;

            case BossDirectionPoint.Direction.East:
                animator.SetBool("pose3", true);
                //Audio este
                break;

            case BossDirectionPoint.Direction.West:
                animator.SetBool("pose4", true);
                //Audio oeste
                break;
        }
        //ResetBools();
    }
    public void ResetBools()
    {
        animator.SetBool("pose1", false);
        animator.SetBool("pose2", false);
        animator.SetBool("pose3", false);
        animator.SetBool("pose4", false);
    }
}