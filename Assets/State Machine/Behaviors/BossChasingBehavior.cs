using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossChasingBehavior : ChasingBehavior
{
    private float cooldown;
    private float currentCooldown;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        cooldown = 5.0f;
        currentCooldown = 0.0f;
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
            Debug.Log("test");
        }
    }
}
