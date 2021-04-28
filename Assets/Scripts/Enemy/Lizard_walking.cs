using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard_walking : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Lizard me;
    private GameObject[] players;
    public float attackRange = 3f;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        rb = animator.GetComponent<Rigidbody2D>();
        me = animator.GetComponent<Lizard>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (GameObject player in players)
        {
            if (Vector2.Distance(player.transform.position, rb.position) <= attackRange)
            {
                Debug.Log("Lizard, got to attack");
                animator.SetTrigger("Attack");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
