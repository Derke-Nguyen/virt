using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleDarkKnivesState : RippleBaseState
{
    public override void EnterState(Ripple ripple)
    {
        ripple.turnOffLights();
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        
    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        
    }

    public override void Update(Ripple ripple)
    {
        ripple.TransitionToState(ripple.FollowState);
    }
}
