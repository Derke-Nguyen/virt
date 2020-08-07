using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingState : PlayerBaseState
{
    Vector3 dashDirection; //Maps the direction of the dash

    public float dashDuration = 0.2f; //Length of the dash in seconds per frame
    public float dashTime; //Counter of dash length
    public float dashSpeed = 20; //Dash speed

    public bool dashFinished; //Checks if currently dashing, used to transition back to idle
    
    //Calculates the direction and speed of the dash, marks the player as dashing
    public override void EnterState(PlayerController player)
    {
        dashDirection = player.direction;
        dashTime = dashDuration;
        dashFinished = false;
    }


    //Moves the player if dash is occuring, otherwise transition back to idle
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


    //TODO: dash should end on collision with object in game world
    public override void OnCollisionEnter(PlayerController player)
    {
        
    }


    //Previous iteration of jump dash, currently unneeded
    public override void Update(PlayerController player)
    {
        //if (dashFinished)
        //{
        //    temp
        //    if (player.Rigidbody.position.y - 1 < 0.1)
        //    {
        //        player.TransitionToState(player.IdleState);
        //    }
        //    else
        //        player.TransitionToState(player.JumpingState);
        //}

        if (Input.GetMouseButton(0) && PlayerIdleState.swingCD <= 0)
        {
            PlayerIdleState.swingCD = PlayerIdleState.swingCD_value;
            player.TransitionToState(player.MeleeState);

            if(PlayerIdleState.swingCD > 0)
            {
                PlayerIdleState.swingCD -= Time.deltaTime;
            }
        }
    }
}
