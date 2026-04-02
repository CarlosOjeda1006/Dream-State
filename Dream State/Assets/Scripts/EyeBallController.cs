using UnityEngine;


public class EyeBallController : FloatingEyeEnemy
{
    private Animator animator;

    protected override void Start()
    {
        base.Start(); 
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        bool previousDetected = hasDetectedPlayer;

        base.Update();

        // JUST detected player this frame
        if (!previousDetected && hasDetectedPlayer)
        {
            animator.SetBool("isSuspicious", true);
        }
    }
    protected override void OnAttack()
    {
        Debug.Log("Animation attack triggered");
        animator.SetBool("isAttacking", true);
    }
}
