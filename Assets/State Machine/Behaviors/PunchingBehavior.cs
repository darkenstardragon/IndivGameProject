using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBehavior : StateMachineBehaviour
{
    private Transform detector;
    private float punchTimer;
    private const float returnTimer = 0.9f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        punchTimer = 0.0f;
        HealthBar hb = animator.GetComponent<HealthBar>();
        detector = hb.EnemyDetector;
        Debug.Log("punch");
        detector.SendMessage("BossSwipe", 10);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        punchTimer += Time.deltaTime;
        detector.SendMessage("ResetAttackTime");
        if (punchTimer >= returnTimer)
        {
            animator.SetBool("isPunching", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        detector.SendMessage("StartAttacking");
        detector.SendMessage("SetTimerActive", true);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
