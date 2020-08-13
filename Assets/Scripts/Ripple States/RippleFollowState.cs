using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleFollowState : RippleBaseState
{
    Coroutine currentCoroutine;
    float maxTimeSpentInState;
    public override void EnterState(Ripple ripple)
    {
        maxTimeSpentInState = 3f;
        currentCoroutine = ripple.StartCoroutine(ripple.UpdatePath()); // pathfinds towards player
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        
    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        
    }

    public override void Update(Ripple ripple)
    {
        maxTimeSpentInState -= Time.deltaTime;
        maxTimeSpentInState = Mathf.Clamp(maxTimeSpentInState, 0, 10);
        if (maxTimeSpentInState == 0)
        {
            ripple.StopAllCoroutines();
            ripple.pauseNavMesh();
            ripple.TransitionToState(ripple.SwingState);
        }
    }
}
