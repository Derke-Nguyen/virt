﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class rippleProjectile : MonoBehaviour
{
    public NavMeshAgent pathfinder;
    public Transform playerTransform;
    public Vector3 unpausedSpeed = Vector3.zero;
    float speed;

    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private float timer;

    Coroutine currentCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        wanderTimer = 0.8f;
        timer = wanderTimer;
        wanderRadius = 20f;
        pathfinder = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        speed = 11f;
        pathfinder.updateRotation = false;
        
        //currentCoroutine = StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((playerTransform.position - transform.position).magnitude < 14f)
        {
                pathfinder.SetDestination(playerTransform.position);
                if (pathfinder.velocity.sqrMagnitude > Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.LookRotation(pathfinder.velocity.normalized);
                }
            
        }
        else if (timer >= wanderTimer)
        {
            
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            pathfinder.SetDestination(newPos);
            Debug.Log(newPos);
            if (pathfinder.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(pathfinder.velocity.normalized);
            }
            timer = 0;
        }

    }
    public void pauseNavMesh()
    {
        pathfinder.velocity = Vector3.zero;
        pathfinder.isStopped = true;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);

        return navHit.position;
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
            if (pathfinder.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(pathfinder.velocity.normalized);
            }

            unpausedSpeed = pathfinder.velocity;
            yield return new WaitForSeconds(refreshRate);

        }
    }
}
