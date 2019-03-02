using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyMovement : MonoBehaviour
{

    public Transform Player;
    public float MoveSpeed = 4;
    public float MaxDist = 5;
    public float MinDist = 2;
    public float lookDistance = 10.0f;

    public CharacterController controller;

    

    private Vector3 dir = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {


        transform.LookAt(Player);
        if (Vector3.Distance(transform.position, Player.position) >= MinDist && Vector3.Distance(transform.position, Player.position) < lookDistance)
        {
            dir = Vector3.Normalize(Player.position - transform.position);
            
            controller.Move(dir * Time.deltaTime);
            


            if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
            {
                print("Reached");
            }

        }
    }

    
}