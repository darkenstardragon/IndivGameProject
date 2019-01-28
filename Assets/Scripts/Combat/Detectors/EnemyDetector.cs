using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private float damage = 10;
    private float timer = 1.0f;
    private bool timerStart = false;
    private Collider player;

    private bool stopAttacking = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            
            timerStart = true;
            player = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            timerStart = false;
            timer = 1.0f;
        }
    }

    private void Update()
    {
        if (timerStart)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                if(!stopAttacking)
                    player.SendMessage("TakeDamage", damage);
                timer = 1.0f;
            }
            print(timer);
        }
    }

    private void SetStopAttacking()
    {
        stopAttacking = true;
    }
}
