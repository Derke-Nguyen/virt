using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1SwingState : Enemy1BaseState
{
    float speed;
    Vector3 direction;
    Quaternion startingRotation;
    float rotationAmount;
    
    public override void EnterState(Enemy1 enemy)
    {
        enemy.pausePathFinder = true;

        startingRotation = enemy.centralAxis.transform.rotation;
        rotationAmount = 0;
        direction = enemy.transform.forward;
        speed = 25;
        enemy.equipWeapon();
    }

    public override void FixedStateUpdate(Enemy1 enemy)
    {
        

    }

    public override void OnCollisionEnter(Enemy1 enemy)
    {
        
    }

    public override void Update(Enemy1 enemy)
    {
        if (rotationAmount >= 360)
        {
            enemy.centralAxis.transform.rotation = startingRotation;
            enemy.pausePathFinder = false;
            if ((enemy.playerTransform.position - enemy.transform.position).magnitude < 5f)
            {
                enemy.TransitionToState(enemy.SummonState);
            }
            else
            {
                enemy.TransitionToState(enemy.ChaseState);
            }
        }
        else
        {
            enemy.centralAxis.transform.Rotate(Vector3.up * Time.deltaTime * 360, Space.World);
            enemy.transform.Translate(direction * Time.deltaTime * speed, Space.World);
            rotationAmount += Time.deltaTime * 360;
        }
    }
}
