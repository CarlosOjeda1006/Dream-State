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

        //  DETECTADO (transición)
        animator.SetBool("isAttacking", hasDetectedPlayer && !isStunned);

        //  SOSPECHA
        if (!hasDetectedPlayer && distanceSqr <= detectionRangeSqr * 2.5f)
        {
            animator.SetBool("isSuspicious", true);
        }
        else
        {
            animator.SetBool("isSuspicious", false);
        }

        //  PIERDE AL JUGADOR
        if (previousDetected && !hasDetectedPlayer)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isSuspicious", false);
        }

    }

}
