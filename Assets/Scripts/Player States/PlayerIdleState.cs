using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public static float dashCD_value = 1;
    public static float swingCD_value = 0.7f;

    public static float dashCD = 0; //Global variable since Idle state and Jump state should share the same dash cooldown
    public static float swingCD = 0; //Global variable since Idle state and Dash state should share the same dash coodown

    Interactable interactable;

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
    public override void Update(PlayerController player)
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Jumps when space is pressed
        {
            player.Rigidbody.AddForce(Vector3.up * 500);
            player.TransitionToState(player.JumpingState);
        }
        else if (Input.GetMouseButton(1)) //Dashes when right mouse button is pressed
        {
            //Debug.Log(dashCD);
            if (dashCD <= 0)
            {
                dashCD = dashCD_value;
                player.TransitionToState(player.DashingState);
            }
        }
        else if (Input.GetMouseButton(0)) //Swings when left mouse button is pressed
        {
            //Debug.Log(swingCD);
            if(swingCD <= 0)
            {
                swingCD = swingCD_value;
                player.TransitionToState(player.MeleeState);
            }
        }
        else if (Input.GetKey("z")) //Interacts with objects when z is pressed
        {
            //We create a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //If the ray hits
            if(Physics.Raycast(ray, out hit, 100))
            {
                interactable = hit.collider.GetComponent<Interactable>();
                if(interactable != null)
                {
                    interactable.Interact(); //Object should light up if pointed to
                }
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
