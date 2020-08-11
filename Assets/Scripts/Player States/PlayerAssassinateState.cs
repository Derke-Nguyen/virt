using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssassinateState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        player.canAssassinate = false;
        player.assassinate();
    }

    public override void FixedStateUpdate(PlayerController player)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter(PlayerController player)
    {
        //throw new System.NotImplementedException();
    }

    public override void Update(PlayerController player)
    {
        player.TransitionToState(player.IdleState);
    }
}
