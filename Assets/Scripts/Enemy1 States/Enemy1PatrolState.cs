﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1PatrolState : Enemy1BaseState
{
    Transform enemy1Transform;
    int startingIndex;
    Vector3 targetWayPoint;
    Coroutine currentCoroutine;
    float minDistance;

    public override void EnterState(Enemy1 enemy)
    {
        //finds nearest waypoint based on distance from enemy1
        startingIndex = 0;
        enemy1Transform = enemy.transform;
        minDistance = (enemy1Transform.position - enemy.waypoints[0]).magnitude;
        for (int i = 1; i < enemy.waypoints.Length; ++i)
        {
            if ((enemy1Transform.position - enemy.waypoints[i]).magnitude < minDistance)
            {
                minDistance = (enemy1Transform.position - enemy.waypoints[i]).magnitude;
                startingIndex = i;
            }
        }
        currentCoroutine = enemy.StartCoroutine(enemy.FollowPath(enemy.waypoints, startingIndex)); //starts patrolling
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

        if (enemy.canSeePlayer())
        {
            enemy.StopAllCoroutines();
            enemy.pauseNavMesh();
            enemy.TransitionToState(enemy.NoticeState);
        }
    }
}
