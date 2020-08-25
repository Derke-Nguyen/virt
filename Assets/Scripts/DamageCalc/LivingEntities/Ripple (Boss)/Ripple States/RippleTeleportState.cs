using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleTeleportState : RippleBaseState
{
    int teleportNum = 0;
    Coroutine pause;
    public override void EnterState(Ripple ripple)
    {
        ripple.deactivateLight();
        teleportNum = (teleportNum % 5) + 1;
        //ripple.pausedState = true;
        pause = ripple.StartCoroutine(ripple.PauseState(0.35f));
    }

    public override void FixedStateUpdate(Ripple ripple)
    {

    }

    public override void OnCollisionEnter(Ripple ripple)
    {

    }

    public override void Update(Ripple ripple)
    {
        if (ripple.isDark)
        {
            Vector3 dirToPlayer = (ripple.playerTransform.position - ripple.transform.position).normalized;
            Vector3 destination = ripple.vectorDestination(ripple.playerTransform.position, dirToPlayer, 15f);
            if (Mathf.Abs(Mathf.Abs(destination.x) - 70) <= 10f || Mathf.Abs(Mathf.Abs(destination.z) - 50) <= 10f)
            {
                ripple.TransitionToState(ripple.SwingState);
            }
            else
            {
                ripple.transform.position = destination;
                ripple.transform.forward = -dirToPlayer;
                teleportNum = 0;
                ripple.TransitionToState(ripple.SwingState);
            }
        }
        else
        {
            if (teleportNum != 4)
            {
                //check edges TODO
                ripple.transform.position = ripple.vectorDestination(ripple.playerTransform.position, -ripple.playerTransform.forward, 15f);
                ripple.transform.forward = ripple.playerTransform.forward;
                ripple.TransitionToState(ripple.SwingState);
            }
            else
            {
                ripple.activateLight();
                ripple.TransitionToState(ripple.FollowState);
                teleportNum = 0;
            }
        }
    }
}
