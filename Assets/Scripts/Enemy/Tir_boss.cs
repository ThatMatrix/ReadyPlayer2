using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;

public class Tir_boss : StateMachineBehaviour
{
    Random rand = new Random();
    private float nextSwitchState;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator.GetComponent<Casper>().counterTir = 0;
        nextSwitchState = Time.time + 5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time > nextSwitchState)
        {
            animator.SetBool("Tir",false);

            if (Random.Range(0,3) == 0)
            {
                animator.SetBool("Tp", true);
            }
            else
            {
                animator.SetBool("Dash", true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
