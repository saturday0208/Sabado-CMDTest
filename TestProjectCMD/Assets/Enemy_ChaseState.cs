using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChaseState : StateMachineBehaviour
{
    public float attackRange = 2f;

    Rigidbody2D rb2d;
    Enemy enemy;

    Transform playerTarget;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2d = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Enemy>();


        if(enemy.detectedPlayer != null)
        {
            AudioManager.Instance.PlaySFX("Bark");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy.detectedPlayer != null)
        {
            playerTarget = enemy.detectedPlayer.gameObject.transform;

            if (enemy.combatEntity.isKnockedBack)
            {
                //rb2d.velocity = Vector2.zero;
                animator.SetTrigger("IsHurt");
                return;
            }

            enemy.LookAtPlayer();
            Vector2 target = new Vector2(playerTarget.position.x, rb2d.position.y);
            Vector2 newPosition = Vector2.MoveTowards(new Vector2(rb2d.position.x, rb2d.position.y), target, 4.5f * Time.fixedDeltaTime);
            rb2d.MovePosition(newPosition);

                if (Vector2.Distance(playerTarget.position, rb2d.position) <= attackRange)
                {
                    rb2d.velocity = Vector2.zero;
                    animator.SetTrigger("IsAttack");
                }
        }
        else
        {
            animator.SetTrigger("IsWalk");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.ResetTrigger("IsWalk");
        animator.ResetTrigger("IsAttack");
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
