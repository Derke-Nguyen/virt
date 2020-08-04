using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        //set sprite here
    }

    public override void FixedStateUpdate(PlayerController player)
    {
        
    }

    public override void OnCollisionEnter(PlayerController player)
    {
        player.TransitionToState(player.IdleState);
    }

    public override void Update(PlayerController player)
    {
        Debug.Log(PlayerIdleState.dashCD);
        if (Input.GetMouseButton(1) && PlayerIdleState.dashCD == 0)
        {
            PlayerIdleState.dashCD= 100;
            player.TransitionToState(player.DashingState);
        }
        if(PlayerIdleState.dashCD > 0)
        {
            --PlayerIdleState.dashCD;
        }
    }
}
