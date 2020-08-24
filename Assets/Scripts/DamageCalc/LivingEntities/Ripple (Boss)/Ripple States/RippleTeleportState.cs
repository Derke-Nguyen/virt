using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleTeleportState : RippleBaseState
{
    int teleportNum = 0;
    Coroutine pause;
    public override void EnterState(Ripple ripple)
    {
        teleportNum = (teleportNum % 5) + 1;
        ripple.pausedState = true;
        pause = ripple.StartCoroutine(ripple.PauseState(0.6f));
    }

    public override void FixedStateUpdate(Ripple ripple)
    {

    }

    public override void OnCollisionEnter(Ripple ripple)
    {

    }

    public override void Update(Ripple ripple)
    {
        if (teleportNum != 4)
        {
            Vector3 ripplePosition = ripple.transform.position;
            Vector3 playerPosition = ripple.playerTransform.position;
            float magnitude = Mathf.Sqrt(Mathf.Pow(ripplePosition.x - playerPosition.x, 2) + Mathf.Pow(ripplePosition.z - playerPosition.z, 2));
            //check edges TODO
            ripple.transform.position = ripple.vectorDestination(ripple.playerTransform.position, -ripple.playerTransform.forward,15f);
            ripple.transform.forward = ripple.playerTransform.forward;
            ripple.TransitionToState(ripple.SwingState);
        }
        else
        {
            ripple.TransitionToState(ripple.FollowState);
        }
    }
}
