using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Ripple : LivingEntity
{
    private RippleBaseState currentState;
    public RippleBaseState CurrentState
    {
        get { return currentState; }
    }

    public readonly RippleFollowState FollowState = new RippleFollowState();
    public readonly RippleSwingState SwingState = new RippleSwingState();
    public readonly RippleSummonState SummonState = new RippleSummonState();
    public readonly RippleDarkKnivesState DarkKnivesState = new RippleDarkKnivesState();
    public readonly RippleAssassinState AssassinState = new RippleAssassinState();
    public readonly RippleDodgeState DodgeState = new RippleDodgeState();

    public NavMeshAgent pathfinder;
    public Transform playerTransform;
    public Vector3 unpausedSpeed = Vector3.zero;

    public Transform weaponHold;
    public Transform centralAxis;
    public Blade equippedBlade;
    public Blade blade;

    public float backstabDistance;
    public float backstabAngle;

    public LayerMask viewMask;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        backstabDistance = 11f;
        backstabAngle = 92.3f;
        pathfinder = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        TransitionToState(FollowState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update(this); //do action based on state
        if (playerCanBackStab())
        {
            Debug.Log("Behind");
        }
    }

    public void FixedUpdate()
    {
        currentState.FixedStateUpdate(this); //do action based on state
    }

    public void TransitionToState(RippleBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public bool playerCanBackStab()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < backstabDistance)
        {
            Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
            float angleBetweenEnemy1AndPlayer = Vector3.Angle(-transform.forward, dirToPlayer);
            if (angleBetweenEnemy1AndPlayer < backstabAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, playerTransform.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void equipWeapon()
    {
        if (!blade)
        {
            blade = Instantiate(equippedBlade, weaponHold.position, weaponHold.rotation);
            blade.transform.parent = weaponHold;
        }

    }

    public void pauseNavMesh()
    {
        pathfinder.velocity = Vector3.zero;
        pathfinder.isStopped = true;
    }

    //pathfinds towards player every 0.1s
    public IEnumerator UpdatePath()
    {
        float refreshRate = 0.1f;
        Vector3 targetPosition;
        while (playerTransform != null)
        {
            targetPosition = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);

            //Start pathfinding again if it was stopped
            if (pathfinder.isStopped)
            {
                pathfinder.isStopped = false;
                pathfinder.velocity = unpausedSpeed;
            }
            pathfinder.SetDestination(targetPosition);
            unpausedSpeed = pathfinder.velocity;
            yield return new WaitForSeconds(refreshRate);

        }
    }
}
