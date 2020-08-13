﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1StunState : Enemy1BaseState
{
    Coroutine currentCoroutine;
    public override void EnterState(Enemy1 enemy)
    {
        enemy.StopAllCoroutines();
        enemy.StartCoroutine(enemy.StunEnemy());
        enemy.TransitionToState(enemy.PatrolState);
        Debug.Log("We got here! StunState!!");
    }

    public override void FixedStateUpdate(Enemy1 enemy)
    {

    }

    public override void OnCollisionEnter(Enemy1 enemy)
    {

    }

    public override void Update(Enemy1 enemy)
    {

    }
}
