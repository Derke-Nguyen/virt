using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssassinState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        player.canAssassinate = false;
        player.assassinate();
    }

    public override void FixedStateUpdate(PlayerController player)
    {

    }

    public override void OnCollisionEnter(PlayerController player)
    {

    }

    public override void Update(PlayerController player)
    {

    }
}
