using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleWideSwingState : RippleBaseState
{
    Blade blade;
    bool activated;
    float angle;
    float numOfTimeDirection = -1;
    int turnsInState = 0;

    float originalSpotLightAngle;
    float originalSpotLightRange;
    float originalSpotLightIntensity;


    public override void EnterState(Ripple ripple)
    {
        activated = false;
        ripple.transform.position = new Vector3(0, 2, 0);
        ++turnsInState;
        if (turnsInState == 1)
        {
            originalSpotLightAngle = ripple.spotlight.spotAngle;
            ripple.spotlight.spotAngle = 180;
            ripple.lightViewAngle = 180;
            originalSpotLightRange = ripple.spotlight.range;
            ripple.spotlight.range = 90f;
            ripple.viewDistance = 90f;
            originalSpotLightIntensity = ripple.spotlight.intensity;
            ripple.spotlight.intensity = 65;
        }
        numOfTimeDirection *= -1;
        ripple.transform.forward = Vector3.forward * numOfTimeDirection;
        angle = 0;
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
        if (!activated && ripple.lightRatio == 1)
        {
            if (turnsInState == 1)
            {
                ripple.centralAxis.forward = -ripple.transform.right;
                ripple.equipWeapon();
                blade = GameObject.FindGameObjectWithTag("Blade").GetComponent<Blade>();
                blade.transform.localScale = new Vector3(0.9375f, 31.40625f, 0.9375f);
                blade.transform.position = blade.transform.position + Vector3.left * 50;
                activated = true;
            }
            else if (turnsInState == 2)
            {
                ripple.centralAxis.forward = -ripple.transform.right;
                ripple.equipWeapon();
                blade = GameObject.FindGameObjectWithTag("Blade").GetComponent<Blade>();
                blade.transform.localScale = new Vector3(0.9375f, 31.40625f, 0.9375f);
                blade.transform.position = blade.transform.position + Vector3.right * 50;
                activated = true;
            }
        }

        if (angle >= 180)
        {
            GameObject.Destroy(GameObject.FindGameObjectWithTag("Blade"));
            ripple.playerVisibleTimer = 0;
            if (turnsInState == 1)
            {
                ripple.TransitionToState(ripple.WideSwingState);
            }
            else
            {
                ripple.spotlight.spotAngle = originalSpotLightAngle;
                ripple.lightViewAngle = originalSpotLightAngle;
                ripple.spotlight.range = originalSpotLightRange;
                ripple.viewDistance = originalSpotLightRange;
                ripple.spotlight.intensity = originalSpotLightIntensity;
                numOfTimeDirection = -1;
                turnsInState = 0;
                ripple.TransitionToState(ripple.FollowState);
            }

        }
        if (activated)
        {
            ripple.centralAxis.transform.Rotate(Vector3.up * Time.deltaTime * 180, Space.World);
            angle += Time.deltaTime * 180;
        }
    }
}
