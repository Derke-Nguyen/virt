using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleSummonState : RippleBaseState
{
    Coroutine currentCoroutine;
    float enteredStateCount = 0;
    public override void EnterState(Ripple ripple)
    {
        ripple.deactivateLight();
        ++enteredStateCount;
        currentCoroutine = ripple.StartCoroutine(ripple.SummonPillars());
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Blade"));
        ripple.summonShockWaves();
        ripple.transform.position = new Vector3(0, 5, 60);
        ripple.GetComponent<MeshRenderer>().enabled = false;
        ripple.GetComponent<CapsuleCollider>().enabled = false;
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        
    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        
    }

    public override void Update(Ripple ripple)
    {
        if (ripple.noShockwaves())
        {
            ripple.StopAllCoroutines();
            ripple.destroyPillars();

            ripple.GetComponent<MeshRenderer>().enabled = true;
            ripple.GetComponent<CapsuleCollider>().enabled = true;
            ripple.activateLight();

            ripple.TransitionToState(ripple.FollowState);
        }
    }
}
