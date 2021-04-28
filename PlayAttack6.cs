using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAttack6 : StateMachineBehaviour
{
     // Playercontrol playercontrol;
     /*
      CameraShake Camera;
      // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
      override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
      {
          playercontrol = GameObject.FindGameObjectWithTag("Player").GetComponent<Playercontrol>();
          Camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();

          playercontrol.Attack6();

      }

      // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
      override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
      {
          animator.SetBool("AttackedBool", false);
      }*/
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack6", false);
       // playercontrol.Attackcount = 1;
    }

}
