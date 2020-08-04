using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingState : PlayerBaseState
{
    Vector3 dashDirection;

    public float startingTime;
    public float dashTime;
    public float dashSpeed;

    public bool dashFinished;
    

    public override void EnterState(PlayerController player)
    {
        dashDirection = player.direction;
        startingTime = 0.2f;
        dashTime = startingTime;
        dashSpeed = 20;

        dashFinished = false;
    }

    public override void FixedStateUpdate(PlayerController player)
    {
        if (!dashFinished && dashTime > 0)
        {
            player.Rigidbody.MovePosition(player.Rigidbody.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            dashTime -= Time.fixedDeltaTime;
        }
        else
        {
            dashFinished = true;
            player.TransitionToState(player.IdleState);
        }
    }

    public override void OnCollisionEnter(PlayerController player)
    {
        
    }

    public override void Update(PlayerController player)
    {
        if (dashFinished)
        {
            //temp
            //if (player.Rigidbody.position.y - 1 < 0.1)
            //{
            //    player.TransitionToState(player.IdleState);
            //}
            //else
            //    player.TransitionToState(player.JumpingState);
        }
    }
}
