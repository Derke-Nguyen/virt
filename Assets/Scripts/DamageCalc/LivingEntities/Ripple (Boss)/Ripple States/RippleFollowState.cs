using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleFollowState : RippleBaseState
{
    Coroutine currentCoroutine;
    float maxTimeSpentInState;
    //float moveCount;
    float enteredStateCount = 0;
    float movesInTheDark = 0;
    bool isSwinging;
    float swingCount;
    public override void EnterState(Ripple ripple)
    {
        ripple.activateLight();
        //moveCount = 0;
        ++enteredStateCount;
        maxTimeSpentInState = 2f;
        isSwinging = false;
        swingCount = 0;
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
        if (enteredStateCount % 9 == 0)
        {
            if (!ripple.isDark)
            {
                ripple.StopAllCoroutines();
                ripple.pauseNavMesh();
                ripple.TransitionToState(ripple.DarkChaseState);
            }
        }
        else if (enteredStateCount % 6 == 0)
        {
            if (!ripple.isDark)
            {
                ripple.StopAllCoroutines();
                ripple.pauseNavMesh();
                ripple.TransitionToState(ripple.SummonState);
            }
        }
        else if (enteredStateCount % 3 == 0)
        {
            if (!ripple.isDark)
            {
                ripple.StopAllCoroutines();
                ripple.pauseNavMesh();
                ripple.TransitionToState(ripple.WideSwingState);
            }
        }
        else if (ripple.previousState == ripple.WideSwingState)
        {
            ripple.StopAllCoroutines();
            ripple.pauseNavMesh();
            ripple.TransitionToState(ripple.LaserMineState);
        }
        else if (maxTimeSpentInState == 0)
        {
            if (!ripple.isDark)
            {
                ripple.StopAllCoroutines();
                ripple.pauseNavMesh();
                ripple.TransitionToState(ripple.TeleportState);
                
            }
        }
        else if (ripple.playerCanBackStab() && enteredStateCount % 2 == 0)
        {
            ripple.StopAllCoroutines();
            ripple.pauseNavMesh();
            ripple.TransitionToState(ripple.DodgeState);
        }
    }
}
