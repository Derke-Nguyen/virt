﻿using System.Collections;
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
        if (teleportNum != 4)
        {
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
