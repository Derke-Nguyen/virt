using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleSwingState : RippleBaseState
{
    float speed;
    Vector3 direction;
    Vector3 startingPosition;
    Quaternion startingRotation;
    Vector3 destination;
    Coroutine pause;

    float rotationAmount;
    public override void EnterState(Ripple ripple)
    {
        ripple.deactivateLight();
        startingPosition = ripple.transform.position;
        startingRotation = ripple.centralAxis.transform.rotation;
        rotationAmount = 0;
        direction = ripple.transform.forward;
        speed = 25;
        ripple.equipWeapon();
        destination = ripple.vectorDestination(startingPosition, direction, 25f);
        ripple.tracker.turnTrackerOn();
        ripple.tracker.movePosition(destination);
        ripple.pausedState = true;
        pause = ripple.StartCoroutine(ripple.PauseState(0.3f));
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        if (Mathf.Abs(Mathf.Abs(ripple.transform.position.x) - 70) <= 2f || Mathf.Abs(Mathf.Abs(ripple.transform.position.z) - 50) <= 2f)
        {
            ripple.centralAxis.transform.rotation = startingRotation;
            ripple.tracker.turnTrackerOff();
            GameObject.Destroy(GameObject.FindGameObjectWithTag("Blade"));
            ripple.pauseNavMesh();
            ripple.StopAllCoroutines();
            if (ripple.isDark)
            {
                if (!ripple.lightBladeActivated)
                {
                    ripple.StartCoroutine(ripple.summonLightBlades());
                }
                ripple.TransitionToState(ripple.DarkChaseState);
            }
            else
            {
                //ripple.TransitionToState(ripple.LaserMineState);
                ripple.TransitionToState(ripple.DarkChaseState);
            }
        }
        if (Mathf.Abs(ripple.transform.position.x - destination.x) <= 0.5f && Mathf.Abs(ripple.transform.position.z - destination.z) <= 0.5f)
        {
            //rotationAmount >= 360
            ripple.centralAxis.transform.rotation = startingRotation;
            ripple.tracker.turnTrackerOff();
            //ripple.TransitionToState(ripple.TeleportState);
            //ripple.TransitionToState(ripple.WideSwingState);
            //ripple.TransitionToState(ripple.LaserMineState);
            GameObject.Destroy(GameObject.FindGameObjectWithTag("Blade"));
            ripple.pauseNavMesh();
            ripple.StopAllCoroutines();
            if (ripple.isDark)
            {
                if (!ripple.lightBladeActivated)
                {
                    ripple.StartCoroutine(ripple.summonLightBlades());
                }
                ripple.TransitionToState(ripple.DarkChaseState);
            }
            else
            {
                //ripple.TransitionToState(ripple.LaserMineState);
                ripple.TransitionToState(ripple.DarkChaseState);
            }
        }
        else
        {
            ripple.centralAxis.transform.Rotate(Vector3.up * Time.fixedDeltaTime * 360, Space.World);
            ripple.transform.Translate(direction * Time.fixedDeltaTime * speed, Space.World);
            rotationAmount += Time.fixedDeltaTime * 360;
        }
    }

    public override void OnCollisionEnter(Ripple ripple)
    {

    }

    public override void Update(Ripple ripple)
    {
        //Debug.Log("x" + Mathf.Abs(ripple.transform.position.x - destination.x));
        //Debug.Log("z" + Mathf.Abs(ripple.transform.position.z - destination.z));
        
    }
}
