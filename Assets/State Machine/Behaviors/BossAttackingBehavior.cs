using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttackingBehavior : StateMachineBehaviour
{
    private const float RETURN_TIME = 2.0f; 
    private float attackTime;
    private NavMeshAgent agent;
    private HealthBar healthBar;
    private Transform detector;

    private bool triggered;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        triggered = false;
        agent = animator.GetComponent<NavMeshAgent>();
        healthBar = animator.GetComponent<HealthBar>();
        detector = healthBar.EnemyDetector;
        agent.speed *= 5;
        attackTime = 0.0f;
        Debug.Log("attack");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTime += Time.deltaTime;

        if(attackTime >= RETURN_TIME / 2)
        {
            if (!triggered)
            {
                detector.SendMessage("BossSwipe");
                triggered = true;
            }
        }
        
        if(attackTime >= RETURN_TIME)
        {
            Debug.Log("leaving attack");
            animator.SetBool("isAttacking", false);
            
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed /= 5;
        detector.SendMessage("StartAttacking");
    }
}
