using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetMouseButton(1))
        {
            player.TransitionToState(player.DashingState);
        }
    }
}
