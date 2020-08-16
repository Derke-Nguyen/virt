using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleSummonState : RippleBaseState
{
    Coroutine currentCoroutine;
    float enteredStateCount = 0;
    public override void EnterState(Ripple ripple)
    {
        ++enteredStateCount;
        currentCoroutine = ripple.StartCoroutine(ripple.SummonPillars());
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Blade"));
        ripple.transform.position = new Vector3(0, ripple.transform.position.y, 0);
        ripple.summonProjectiles();
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        
    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        
    }

    public override void Update(Ripple ripple)
    {
        ripple.transform.position = new Vector3(0, ripple.transform.position.y, 0);
        if (ripple.pillarsDone)
        {
            ripple.StopAllCoroutines();
            ripple.pillarsDone = false;
            if (enteredStateCount % 2 != 0)
                ripple.TransitionToState(ripple.SmashState);
            else
                ripple.TransitionToState(ripple.DarkKnivesState);
        }
    }
}
