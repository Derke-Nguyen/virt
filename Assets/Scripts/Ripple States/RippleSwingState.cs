using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleSwingState : RippleBaseState
{
    float speed;
    Vector3 direction;
    Quaternion startingRotation;
    float rotationAmount;
    public override void EnterState(Ripple ripple)
    {
        startingRotation = ripple.centralAxis.transform.rotation;
        rotationAmount = 0;
        direction = ripple.transform.forward;
        speed = 25;
        ripple.equipWeapon();
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        throw new System.NotImplementedException();
    }

    public override void Update(Ripple ripple)
    {
        if (rotationAmount >= 360)
        {
            ripple.centralAxis.transform.rotation = startingRotation;
            ripple.TransitionToState(ripple.FollowState);
        }
        else
        {
            ripple.centralAxis.transform.Rotate(Vector3.up * Time.deltaTime * 360, Space.World);
            ripple.transform.Translate(direction * Time.deltaTime * speed, Space.World);
            rotationAmount += Time.deltaTime * 360;
        }
    }
}
