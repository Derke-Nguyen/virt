using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1SummonState : Enemy1BaseState
{
    public override void EnterState(Enemy1 enemy)
    {
        enemy.pausePathFinder = true;

        
    }

    public override void FixedStateUpdate(Enemy1 enemy)
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter(Enemy1 enemy)
    {
        throw new System.NotImplementedException();
    }

    public override void Update(Enemy1 enemy)
    {
        
    }
}
