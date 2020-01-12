using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossChasingBehavior : ChasingBehavior
{
    // private float detectorTimer = 0.0f;

    private const float GAP_DIST = 1f;
    private float cooldown;
    private float currentCooldown = 0.0f;
    private Transform detector;
    private EnemyDetector detectorScript;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        detector = GetHealthBar.EnemyDetector;
        detectorScript = detector.GetComponent<EnemyDetector>();
        cooldown = 5.0f;
        //currentCooldown = 0.0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (currentCooldown < cooldown)
        {
            currentCooldown += Time.deltaTime;
        }
        else
        {
            currentCooldown = 0.0f;
            Debug.Log("go to attack");
            animator.SetBool("isAttacking", true);
        }
        //Debug.Log(detectorScript.GetTimer);
        
        if(detectorScript.GetTimer <= 0.1f)
        {
            detector.SendMessage("SetStopAttacking");
            detector.SendMessage("ResetAttackTime");
            detector.SendMessage("SetTimerActive", false);
            animator.SetBool("isPunching", true);
            //detector.SendMessage("")
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        detector.SendMessage("SetStopAttacking");
        GetAgent.destination = Player.position - Vector3.Normalize(Player.position - animator.transform.position) * GAP_DIST;
    }
}
