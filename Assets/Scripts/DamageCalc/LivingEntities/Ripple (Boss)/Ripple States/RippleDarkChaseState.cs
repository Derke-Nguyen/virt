using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleDarkChaseState : RippleBaseState
{
    Coroutine currentCoroutine;
    List<Vector3> potentialDestinations = new List<Vector3>();
    int maxIndex;
    float maxDistance;
    public override void EnterState(Ripple ripple)
    {
        ripple.turnOffLights();
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Blade"));
        ripple.deactivateLight();
        ripple.summon8Pillars();
        maxIndex = 0;
        maxDistance = 0;
        if (potentialDestinations.Count != 4)
        {
            potentialDestinations.Add(new Vector3(-65f, 2f, 40f));
            potentialDestinations.Add(new Vector3(65f, 2f, 40f));
            potentialDestinations.Add(new Vector3(65f, 2f, -40f));
            potentialDestinations.Add(new Vector3(-65f, 2f, -40f));
        }
        for (int i = 0; i < 4; ++i)
        {
            if ((ripple.playerTransform.position - potentialDestinations[i]).magnitude > maxDistance)
            {
                maxIndex = i;
                maxDistance = (ripple.playerTransform.position - potentialDestinations[i]).magnitude;
            }
        }
        
        ripple.transform.position = potentialDestinations[maxIndex];
        currentCoroutine = ripple.StartCoroutine(ripple.UpdatePath());
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        
    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        
    }

    public override void Update(Ripple ripple)
    {
        if ((ripple.playerTransform.position - ripple.transform.position).magnitude <= 10f)
        {
            ripple.StopAllCoroutines();
            ripple.pauseNavMesh();
            ripple.TransitionToState(ripple.TeleportState);
        }
    }
}
