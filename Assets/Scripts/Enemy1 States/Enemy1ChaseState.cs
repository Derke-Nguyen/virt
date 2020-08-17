using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1ChaseState : Enemy1BaseState
{
    Coroutine currentCoroutine;
    float maxTimeSpentInState;
    Vector3 originalVelocity = Vector3.zero;
    public override void EnterState(Enemy1 enemy)
    {
        //TBD
        maxTimeSpentInState = 3f;
        enemy.playerVisibleTimer = 0;

        enemy.spotlight.color = Color.blue;
        enemy.changeSpeed(2f);
        currentCoroutine = enemy.StartCoroutine(enemy.UpdatePath()); // pathfinds towards player
    }

    public override void FixedStateUpdate(Enemy1 enemy)
    {


    }

    public override void OnCollisionEnter(Enemy1 enemy)
    {

    }

    public override void Update(Enemy1 enemy)
    {
        //maxTimeSpentInState -= Time.deltaTime;
        //maxTimeSpentInState = Mathf.Clamp(maxTimeSpentInState, 0, 10);
        //if (maxTimeSpentInState == 0)
        //{
        //    enemy.changeSpeed(-2f);
        //    enemy.TransitionToState(enemy.SwingState);
        //}
    }
}
