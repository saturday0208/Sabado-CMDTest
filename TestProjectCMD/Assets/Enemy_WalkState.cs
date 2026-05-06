using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WalkState : StateMachineBehaviour
{
    Rigidbody2D rb2d;
    Enemy enemy;

    bool isCheckingLedge;
    bool isCheckingWall;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2d = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Enemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy.combatEntity.isKnockedBack)
        {
            //rb2d.velocity = Vector2.zero;
            animator.SetTrigger("IsHurt");
            return;
        }

        rb2d.velocity = new Vector2(enemy.facingDirection * 3f, rb2d.velocity.y);


        isCheckingLedge = enemy.CheckLedge();
        isCheckingWall = enemy.CheckWall();

        if (!isCheckingLedge || isCheckingWall)
        {
            enemy.Flip();
        }

        if (enemy.hasDetectedPlayer)
        {
            animator.SetTrigger("IsChase");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("IsChase");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
