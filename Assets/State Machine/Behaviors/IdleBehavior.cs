using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleBehavior : StateMachineBehaviour
{
    private NavMeshAgent agent;

    private Transform player;
    private CharacterController controller;
    private HealthBar healthBar;

    public float lookDistance = 10.0f;

    private bool isDead = false;

    private bool isKnockBack = false;
    private float knockBackTime = 0.0f;
    private const float KNOCKBACK_SPEED = 30.0f;

    private bool readyForNewDestination;
    private float newDestinationTime = 0.0f;
    private const float DESTINATION_RESET_TIME = 5.0f;

    private Vector3 dir = Vector3.zero;
    private Vector3 originalPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        readyForNewDestination = true;
        Debug.Log("idle");
        originalPosition = animator.transform.position;
        healthBar = animator.GetComponent<HealthBar>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = animator.GetComponent<CharacterController>();
        agent = animator.GetComponent<NavMeshAgent>();
        if(agent == null)
        {
            agent = animator.GetComponentInParent<NavMeshAgent>();
        }
        agent.speed = agent.speed / 2;
        Delay(1.0f);
        agent.isStopped = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("isChasing", true);
        }
        */
        if (!isDead)
        {
            if (isKnockBack)
            {
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
            else
            {
                if (readyForNewDestination)
                {
                    agent.destination = originalPosition + FindNewDestination();
                    readyForNewDestination = false;
                }
                else
                {
                    
                    if(newDestinationTime > DESTINATION_RESET_TIME)
                    {
                        Debug.Log("reached!");
                        readyForNewDestination = true;
                        Delay(1.0f);
                        newDestinationTime = 0.0f;
                    }
                    else
                    {
                        //Debug.Log("not reached");
                        newDestinationTime += Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            animator.SetBool("isDead", true);
        }

        if (Vector3.Distance(animator.transform.position, player.transform.position) <= lookDistance)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed = agent.speed * 2;
    }

    private Vector3 FindNewDestination()
    {
        float x = Random.Range(3.5f, 4.0f);
        float z = Random.Range(3.5f, 4.0f);
        float xPosChance = Random.Range(0, 100);
        float zPosChance = Random.Range(0, 100);
        if(xPosChance <= 50.0f)
        {
            x = -x;
        }
        if(zPosChance <= 50.0f)
        {
            z = -z;
        }
        Vector3 des = new Vector3(x, 0, z);
        return des;
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

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
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
