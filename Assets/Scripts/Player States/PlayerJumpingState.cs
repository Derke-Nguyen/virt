using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

//Are we going to have a jumping mechanic in the game?

public class PlayerJumpingState : PlayerBaseState
{
    //TODO: Add jumping animation
    public override void EnterState(PlayerController player)
    {
        //set sprite here
    }

    //Nothing should go here
    public override void FixedStateUpdate(PlayerController player)
    {
        
    }

    //TODO: Jump should only end on collision with ground or platform
    //Currently, jump ends on collision with anything
    public override void OnCollisionEnter(PlayerController player)
    {
        player.TransitionToState(player.IdleState);
    }

    //Allows player to use dash in midair
    //Shares a cooldown with Idle state using Idle state's dashCD
    public override void Update(PlayerController player)
    {
        //Debug.Log(PlayerIdleState.dashCD);
        if (Input.GetMouseButton(1) && PlayerIdleState.dashCD <= 0)
        {
            PlayerIdleState.dashCD = PlayerIdleState.dashCD_value;
            player.TransitionToState(player.DashingState);
        }
        if(PlayerIdleState.dashCD > 0)
        {
            PlayerIdleState.dashCD -= Time.deltaTime;
        }
    }
}
