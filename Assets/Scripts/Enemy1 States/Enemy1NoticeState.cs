    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1NoticeState : Enemy1BaseState
{
    public override void EnterState(Enemy1 enemy)
    {
        
    }

    public override void FixedStateUpdate(Enemy1 enemy)
    {
        
    }

    public override void OnCollisionEnter(Enemy1 enemy)
    {
        
    }

    public override void Update(Enemy1 enemy)
    {
        enemy.lightControl();
        enemy.NavMove();
        //Debug.Log(((enemy.playerTransform.position - enemy.transform.position).magnitude));
        if (enemy.changeToChase())
        {
            enemy.TransitionToState(enemy.ChaseState);
        }
        else if ((enemy.playerTransform.position - enemy.transform.position).magnitude >= 20f)
        {
            
            enemy.TransitionToState(enemy.PatrolState);
        }
    }
}
