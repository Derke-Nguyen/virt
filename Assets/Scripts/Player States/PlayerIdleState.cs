using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public static int dashCD = 0;

    public override void EnterState(PlayerController player)
    {
        //set sprite here
    }

    public override void FixedStateUpdate(PlayerController player)
    {
        
    }

    public override void OnCollisionEnter(PlayerController player)
    {
        
    }

    public override void Update(PlayerController player)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Rigidbody.AddForce(Vector3.up * 500);
            player.TransitionToState(player.JumpingState);
        }
        else if (Input.GetMouseButton(1))
        {
            Debug.Log(dashCD);
            if (dashCD == 0)
            {
                dashCD = 100;
                player.TransitionToState(player.DashingState);
            }
        }
        if(dashCD > 0)
        {
            --dashCD;
        }
    }
}
