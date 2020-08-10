using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1ChaseState : Enemy1BaseState
{
    Coroutine currentCoroutine;
    public override void EnterState(Enemy1 enemy)
    {
        enemy.spotlight.color = Color.blue;
        enemy.changeSpeed(2);
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
        //enemy.NavMove();
    }
}
