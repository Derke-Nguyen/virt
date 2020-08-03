using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        //set sprite here
    }   

    public override void OnCollisionEnter(PlayerController player)
    {
        
    }

    public override void Update(PlayerController player)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
              
            player.Rigidbody.AddForce(Vector3.up * player.jumpForce);
            player.TransitionToState(player.JumpingState);
        }
    }
}
