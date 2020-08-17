using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1NoticeState : Enemy1BaseState
{
    Coroutine currentCoroutine;
    public override void EnterState(Enemy1 enemy)
    {
        currentCoroutine = enemy.StartCoroutine(enemy.UpdatePath());
    }

    public override void FixedStateUpdate(Enemy1 enemy)
    {

    }

    public override void OnCollisionEnter(Enemy1 enemy)
    {

    }

    public override void Update(Enemy1 enemy)
    {
        enemy.lightControl(); //changes color of light

        if (enemy.changeToChase()) //if light is completely red
        {
            enemy.StopAllCoroutines();
            enemy.TransitionToState(enemy.ChaseState);
        }
        else if ((enemy.playerTransform.position - enemy.transform.position).magnitude >= 15f || enemy.obstacleInFront())
        {
            enemy.StopAllCoroutines();
            enemy.pauseNavMesh();
            enemy.TransitionToState(enemy.PatrolState);
        }
    }
}
