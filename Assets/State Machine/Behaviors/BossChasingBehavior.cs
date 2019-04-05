using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossChasingBehavior : ChasingBehavior
{
    private float cooldown;
    private float currentCooldown;
    private Transform detector;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        detector = GetHealthBar.EnemyDetector;
        cooldown = 5.0f;
        currentCooldown = 0.0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (currentCooldown < cooldown)
        {
            currentCooldown += Time.deltaTime;
        }
        else
        {
            currentCooldown = 0.0f;
            Debug.Log("go to attack");
            animator.SetBool("isAttacking", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        detector.SendMessage("SetStopAttacking");
    }
}
