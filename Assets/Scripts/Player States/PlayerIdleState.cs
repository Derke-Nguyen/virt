using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public static float dashCD_value = 1;
    public static float swingCD_value = 0.7f;

    public static float dashCD = 0; //Global variable since Idle state and Jump state should share the same dash cooldown
    public static float swingCD = 0; //Global variable for melee swing cooldown



    //TODO: Add idling animations
    public override void EnterState(PlayerController player)
    {
        //set sprite here
    }

    //Nothing should go here
    public override void FixedStateUpdate(PlayerController player)
    {
        
    }

    //Nothing should go here
    public override void OnCollisionEnter(PlayerController player)
    {
        
    }


    //Checks for input in order to change state
    //TODO: Add melee attack state
    public override void Update(PlayerController player)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Rigidbody.AddForce(Vector3.up * 500);
            player.TransitionToState(player.JumpingState);
        }
        else if (Input.GetMouseButton(1))
        {
            //Debug.Log(dashCD);
            if (dashCD <= 0)
            {
                dashCD = dashCD_value;
                player.TransitionToState(player.DashingState);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            //Debug.Log(swingCD);
            if(swingCD <= 0)
            {
                swingCD = swingCD_value;
                player.TransitionToState(player.MeleeState);
            }
        }


        //Reducing CD of dash and swing if on CD
        if(dashCD > 0)
        {
            dashCD -= Time.deltaTime;
        }
        if(swingCD > 0)
        {
            swingCD -= Time.deltaTime;
        }

    }
}
