using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SepticEyeSecondPhase : StateMachineBehaviour
{
    private List<GameObject> points;
    private float timeBtwShots;
    private int alternate = 0;
    private SecticEye me;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        me = animator.GetComponent<SecticEye>();
        timeBtwShots = animator.GetComponent<SecticEye>().startTimeBtwShots / 2;
        points = animator.GetComponent<SecticEye>().points;
        me.DoubleShotForce();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timeBtwShots <= 0)
        {
            if (alternate % 2 == 0)
            {
                me.ShootFromLeft();
            }
            else
            {
                me.ShootFromRight();
            }

            alternate += 1;
            timeBtwShots = animator.GetComponent<SecticEye>().startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
