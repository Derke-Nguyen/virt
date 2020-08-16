using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleSmashState : RippleBaseState
{
    Coroutine currentCoroutine;
    public override void EnterState(Ripple ripple)
    {
        currentCoroutine = ripple.StartCoroutine(ripple.findSmashablePillars());
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        
    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        
    }

    public override void Update(Ripple ripple)
    {
        if (ripple.endPillar || ripple.checkIfNoProjectiles())
        {
            ripple.StopAllCoroutines();
            ripple.destroyPillars();
            ripple.endPillar = false;
            ripple.TransitionToState(ripple.FollowState);
        }
    }
}
