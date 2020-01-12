using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttackingBehavior : StateMachineBehaviour
{
    private const float GAP_DIST = 3.0f;
    private Transform player;
    private const float RETURN_TIME = 1.6f; 
    private float attackTime;
    private NavMeshAgent agent;
    private HealthBar healthBar;
    private Transform detector;

    private const float SPEED_MULTIPLIER = 6.0f;
    private float previousSpeed;
    private bool triggered;
    private bool speedBoosted;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        speedBoosted = false;
        triggered = false;
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //agent.destination = player.position + Vector3.Normalize(player.position - animator.transform.position) * GAP_DIST;
        healthBar = animator.GetComponent<HealthBar>();
        detector = healthBar.EnemyDetector;
        previousSpeed = agent.speed;
        //agent.speed = 0.0f;
        agent.speed *= SPEED_MULTIPLIER;
        attackTime = 0.0f;
        Debug.Log("attack");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTime += Time.deltaTime;
        /*
        if (attackTime >= RETURN_TIME / 8)
        {
            if (!speedBoosted)
            {
                agent.speed = previousSpeed * SPEED_MULTIPLIER;
                speedBoosted = true;
            }
        }*/

        if (attackTime >= RETURN_TIME / 2)
        {
            if (!triggered)
            {
                detector.SendMessage("BossSwipe", 30);
                triggered = true;
            }
        }
        
        if(attackTime >= RETURN_TIME)
        {
            //Debug.Log("leaving attack");
            animator.SetBool("isAttacking", false);
            
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed /= SPEED_MULTIPLIER;
        detector.SendMessage("StartAttacking");
        
    }
}
