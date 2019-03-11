using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BetterEnemyMovement : MonoBehaviour
{
    public Transform player;
    public Transform detector;

    private NavMeshAgent agent;
    private CharacterController controller;
    public float lookDistance = 10.0f;
    public float stopDistance = 3.0f;

    private bool isDead = false;

    private bool isKnockBack = false;
    private float knockBackTime = 0.0f;
    private const float KNOCKBACK_SPEED = 30.0f;

    private Vector3 dir = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            if (!isKnockBack)
            {
                agent.isStopped = false;
                transform.LookAt(player);
                if (Vector3.Distance(transform.position, player.position) <= lookDistance)
                {
                    if (!isKnockBack) agent.destination = player.position;
                    if (Vector3.Distance(transform.position, player.position) < stopDistance)
                    {
                        agent.isStopped = true;
                        agent.destination = transform.position;
                        //print("STOP");
                    }
                    else
                    {
                        agent.isStopped = false;
                    }
                }
            }
            else
            {
                agent.isStopped = true;

                detector.transform.SendMessage("ResetAttackTime");
                Vector3 grounddir = player.position - transform.position;
                grounddir.y = transform.position.y -20;
                dir = Vector3.Normalize(grounddir);

                if (knockBackTime > 0)
                {
                    //print("knooooock");
                    //print(knockBackTime);
                    Debug.DrawLine(player.position, transform.position);
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
        
        //if (controller.isGrounded) print("is Grounded");

        //TODO: Short Immobilize after being knocked
    }

    public void SetDead()
    {
        print("dead");
        isDead = true;
        detector.SendMessage("SetStopAttacking");
    }

    private void KnockBack(float time)
    {
        //print("injaa");
        isKnockBack = true;
        knockBackTime = time;
    }
}
