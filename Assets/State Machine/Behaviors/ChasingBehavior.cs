using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasingBehavior : StateMachineBehaviour
{
    public string searchTag;
    public List<GameObject> actors = new List<GameObject>();

    private Transform player;

    private HealthBar healthBar;

    private NavMeshAgent agent;
    private CharacterController controller;
    public float lookDistance = 10.0f;
    public float stopDistance = 3.0f;

    private bool isDead = false;

    private bool isKnockBack = false;
    private float knockBackTime = 0.0f;
    private const float KNOCKBACK_SPEED = 30.0f;

    private Vector3 dir = Vector3.zero;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        healthBar = animator.GetComponent<HealthBar>();
        //detector = animator.GetComponentInChildren;
        controller = animator.GetComponent<CharacterController>();
        agent.isStopped = false;
        Debug.Log("chasing");
        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("isChasing", false);
        }
        */
        if (!isDead)
        {
            if (!isKnockBack)
            {
                agent.isStopped = false;
                animator.transform.LookAt(player);

                if (!isKnockBack) agent.destination = player.position;
                if (Vector3.Distance(animator.transform.position, player.position) < stopDistance)
                {
                    agent.isStopped = true;
                    agent.destination = animator.transform.position;
                    //print("STOP");
                }
                else
                {
                    agent.isStopped = false;
                }

            }
            else
            {
                agent.isStopped = true;
                //Debug.Log(knockBackTime);
                //detector.transform.SendMessage("ResetAttackTime");
                healthBar.ResetAttackTimeHB();
                Vector3 grounddir = player.position - animator.transform.position;
                grounddir.y = animator.transform.position.y - 20;
                dir = Vector3.Normalize(grounddir);

                if (knockBackTime > 0)
                {
                    //print("knooooock");
                    //print(knockBackTime);
                    Debug.DrawLine(player.position, animator.transform.position);
                    controller.Move(-dir * Time.deltaTime * KNOCKBACK_SPEED);

                    knockBackTime -= Time.deltaTime;
                }
                else
                {
                    isKnockBack = false;
                    knockBackTime = 0.0f;
                }
            }
        }

        if (Vector3.Distance(animator.transform.position, player.position) > lookDistance)
        {
            animator.SetBool("isChasing", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //agent.isStopped = true;
    }

    public void SetDead()
    {
        //print("dead");
        isDead = true;
    }

    public void KnockBack(float time)
    {
        //Debug.Log("injaa");
        isKnockBack = true;
        knockBackTime = time;
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
