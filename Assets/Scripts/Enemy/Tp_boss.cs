using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tp_boss : StateMachineBehaviour
{
    private float nextSwitchState;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nextSwitchState = Time.time + 0.5f;
        animator.GetComponent<Casper>().hasTP = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time > nextSwitchState)
        {
            animator.SetBool("Tp", false);
            animator.SetBool("Tir",true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    
}
