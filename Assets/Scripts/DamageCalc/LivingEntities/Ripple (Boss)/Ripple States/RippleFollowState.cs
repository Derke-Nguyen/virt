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
    public override void EnterState(Ripple ripple)
    {
        //moveCount = 0;
        ++enteredStateCount;
        maxTimeSpentInState = 2f;
        if (ripple.isDark)
        {
            ++movesInTheDark;
        }
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
        //Debug.Log(enteredStateCount);
        maxTimeSpentInState -= Time.deltaTime;
        maxTimeSpentInState = Mathf.Clamp(maxTimeSpentInState, 0, 10);
        if (ripple.isDark && movesInTheDark >= 5)
        {
            ripple.turnOnLights();
            ripple.destroyPillars();
        }
        else
        {
            movesInTheDark = 0;
        }
        if (enteredStateCount % 7 == 0)
        {
            if (!ripple.isDark)
            {
                ripple.StopAllCoroutines();
                ripple.pauseNavMesh();
                ripple.TransitionToState(ripple.SummonState);
            }
        }
        if (enteredStateCount % 3 == 0)
        {
            if (!ripple.isDark || ripple.isDark && (ripple.playerTransform.position - ripple.transform.position).magnitude < 25f)
            {
                ripple.StopAllCoroutines();
                ripple.pauseNavMesh();
                ripple.TransitionToState(ripple.DodgeState);
            }
        }
        if (maxTimeSpentInState == 0)
        {
            if (!ripple.isDark || ripple.isDark && (ripple.playerTransform.position - ripple.transform.position).magnitude < 10f)
            {
                ripple.StopAllCoroutines();
                ripple.pauseNavMesh();
                ripple.TransitionToState(ripple.SwingState);
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
