using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDetector : MonoBehaviour
{
    private float damage = 10;
    private float timer = 1.0f;
    private const float resetTime = 1.0f;
    private bool timerStart = false;
    private Collider player;

    private bool stopAttacking = false;
    public Image AttackProgressBar;
    public Image AttackProgressBarBackground;

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
        AttackProgressBar.gameObject.SetActive(timerStart && !stopAttacking);
        AttackProgressBarBackground.gameObject.SetActive(timerStart && !stopAttacking);
        AttackProgressBar.fillAmount = resetTime - timer;
        if (timerStart)
        {
           
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                if(!stopAttacking)
                    player.SendMessage("TakeDamage", damage);
                ResetAttackTime();
            }
        }
    }

    private void SetStopAttacking()
    {
        stopAttacking = true;
    }

    private void ResetAttackTime()
    {
        timer = resetTime;
        //print("reset");
    }
}
