﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleDodgeState : RippleBaseState
{
    float speed;
    Vector3 direction;
    Quaternion startingRotation;
    float rotationAmount;
    Vector3 dirToPlayer;
    public override void EnterState(Ripple ripple)
    {
        startingRotation = ripple.centralAxis.transform.rotation;
        rotationAmount = 0;
        direction = ripple.transform.forward;
        speed = 22;
        ripple.equipWeapon();
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        
    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        
    }

    public override void Update(Ripple ripple)
    {
        if (rotationAmount >= 180)
        {
            Vector3 dirToPlayer = ripple.playerTransform.position - ripple.transform.position;
            float angle = Vector3.Angle(ripple.transform.forward, dirToPlayer);
            if (angle < 10f)
            {
                ripple.blade.transform.parent = null;
                ripple.TransitionToState(ripple.DashState);
            }
            ripple.transform.Rotate(Vector3.up * Time.deltaTime * -360, Space.World);
            rotationAmount += Time.deltaTime * 360;
        }
        else
        {
            ripple.transform.Rotate(Vector3.up * Time.deltaTime * -360, Space.World);
            ripple.transform.Translate(direction * Time.deltaTime * speed, Space.World);
            rotationAmount += Time.deltaTime * 360;
        }
    }
}
