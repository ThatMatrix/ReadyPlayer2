using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_casper : StateMachineBehaviour
{
    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
     override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
         if (animator.GetComponent<Casper>().arrived)
         {
             animator.SetBool("Dash", false);
             animator.SetBool("Tir",true);
         }
     }
    
}
