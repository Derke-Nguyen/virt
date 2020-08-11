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
        //if (originalVelocity != Vector3.zero)
        //{
        //    enemy.pathfinder.velocity = originalVelocity;
        //}
        //enemy.pathfinder.isStopped = false;
        //enemy.pathfinder.updatePosition = true;
        //enemy.pathfinder.isStopped = false;
        //enemy.pathfinder.ResetPath();
        //Debug.Log(enemy.pathfinder.velocity);
        maxTimeSpentInState = 3f;
        enemy.playerVisibleTimer = 0;
        enemy.spotlight.color = Color.blue;
        enemy.changeSpeed(2f);
        currentCoroutine = enemy.StartCoroutine(enemy.UpdatePath());
        //Debug.Log(enemy.pathfinder.velocity);
        
    }

    public override void FixedStateUpdate(Enemy1 enemy)
    {
        
        
    }

    public override void OnCollisionEnter(Enemy1 enemy)
    {
        
    }

    public override void Update(Enemy1 enemy)
    {
        maxTimeSpentInState -= Time.deltaTime;
        maxTimeSpentInState = Mathf.Clamp(maxTimeSpentInState, 0, 10);
        if (maxTimeSpentInState == 0)
        {
            enemy.changeSpeed(-2f);
            //enemy.StopAllCoroutines();
            //originalVelocity = enemy.pathfinder.velocity;
            //enemy.pathfinder.velocity = Vector3.zero;
            //enemy.pathfinder.isStopped = true;
            enemy.TransitionToState(enemy.SwingState);
        }
        //enemy.NavMove();
    }
}
