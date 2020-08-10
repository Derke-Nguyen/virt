using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerMeleeState : PlayerBaseState
{
    //Same format as Dash State
    public bool swingFinished = false;
    public float swingDuration = 0.4f;
    public float swingTime = 0;
    public float swingArc = 600;

    Vector3 EulerAngleVelocityFirstHalf;
    Vector3 EulerAngleVelocitySecondHalf;

    public override void EnterState(PlayerController player)
    {
        player.Rigidbody.isKinematic = true; //Makes rotation not instantaneous
        swingTime = swingDuration;
        swingFinished = false;

        EulerAngleVelocityFirstHalf = new Vector3(0, -swingArc, 0); //Calculates arc for first swing
        EulerAngleVelocitySecondHalf = new Vector3(0, swingArc, 0); //Calculates arc for second swing
    }

    //Changes sword color to red when swinging
    //NOTE: In order to implement this, I had to create a new "Resource" folder and put the two sword materials in that folder
    public override void Update(PlayerController player)
    {
        if(player.GetComponent<WeaponController>().currentWeapon != null)
        {
            //Debug.Log("WE GOT HERE");
            player.GetComponent<WeaponController>().currentWeapon.GetComponentInChildren<Renderer>().material 
                = Resources.Load("SwordSwing", typeof(Material)) as Material;
        }
    }

    public override void OnCollisionEnter(PlayerController player)
    {

    }

    public override void FixedStateUpdate(PlayerController player)
    {
        //Swings forward for first half
        if(!swingFinished && swingTime > swingDuration / 2)
        {
            Quaternion swordSwing = Quaternion.Euler(EulerAngleVelocityFirstHalf * Time.deltaTime);
            player.Rigidbody.MoveRotation(swordSwing * player.Rigidbody.rotation);

            swingTime -= Time.deltaTime;
        }
        //Then backwards for second half
        else if (!swingFinished && swingTime > 0)
        {
            Quaternion swordSwing = Quaternion.Euler(EulerAngleVelocitySecondHalf * Time.deltaTime);
            player.Rigidbody.MoveRotation(swordSwing * player.Rigidbody.rotation);

            swingTime -= Time.deltaTime;
        }
        //When finished, change sword color back and return to idle
        else
        {
            swingFinished = true;
            player.Rigidbody.isKinematic = false;

            player.GetComponent<WeaponController>().currentWeapon.GetComponentInChildren<Renderer>().material
                = Resources.Load("SwordIdle", typeof(Material)) as Material;


            //player.velocity = new Vector3(0, 0, 0);
            player.TransitionToState(player.IdleState);
        }
    }
}
