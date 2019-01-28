using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BetterEnemyMovement : MonoBehaviour
{
    public Transform player;
    public Transform detector;

    NavMeshAgent agent;
    public float lookDistance = 10.0f;
    public float stopDistance = 3.0f;

    private bool stopMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopMoving)
        {
            transform.LookAt(player);
            if (Vector3.Distance(transform.position, player.position) <= lookDistance)
            {
                agent.destination = player.position;
                if (Vector3.Distance(transform.position, player.position) < stopDistance)
                {
                    agent.destination = transform.position;
                }
            }
        }
    }

    void SetDead()
    {
        stopMoving = true;
        detector.SendMessage("SetStopAttacking");
    }
}
