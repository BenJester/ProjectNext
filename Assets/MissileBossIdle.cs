using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBossIdle : StateMachineBehaviour
{
    public float attackPreload;
    float timer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.fixedDeltaTime;
        if (timer > attackPreload)
            animator.SetTrigger("Attack");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
