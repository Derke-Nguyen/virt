using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class Enemy1 : LivingEntity
{
    private Enemy1BaseState currentState;
    public Enemy1BaseState CurrentState
    {
        get { return currentState; }
    }

    public readonly Enemy1PatrolState PatrolState = new Enemy1PatrolState();
    public readonly Enemy1NoticeState NoticeState = new Enemy1NoticeState();
    public readonly Enemy1ChaseState ChaseState = new Enemy1ChaseState();

    public Transform pathHolder;

    public float speed = 5;
    public float waitTime = 0.3f;
    public float turnSpeed = 90;
    

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    Color originalSpotLightColor;

    float viewAngle;

    float timeToSpotPlayer = 2f;
    public float playerVisibleTimer;
    public Vector3[] waypoints;

    public NavMeshAgent pathfinder;
    public Transform playerTransform;
    public Vector3 unpausedSpeed = Vector3.zero;

    public Image Health;
    float damage = 20;

    //Coroutine currentCoroutine;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        viewAngle = spotlight.spotAngle;
        originalSpotLightColor = spotlight.color;

        //put all waypoints in an array
        waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; ++i)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        TransitionToState(PatrolState);
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update(this); //do action based on state
        inTheRed(); //checks if spotlight is red
    }

    public void FixedUpdate()
    {
        currentState.FixedStateUpdate(this); //do action based on state
    }

    public bool inTheRed()
    {
        //based on how red spotlight is, return true
        if ((playerVisibleTimer / timeToSpotPlayer) > 0.75)
        {
            return true;
        }
        else
            return false;

    }

    public void changeSpeed(float changedSpeed)
    {
        pathfinder.speed += changedSpeed; //increase or decrease pathfinding speed
    }

    //changes color of spotlight if player is in spotlight
    public void lightControl()
    {
        if (canSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
        }
        //clamps value to timeToSpotPlayer
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        //changes color gradually based on a fractional value of playerVisibleTimer / timeToSpotPlayer
        spotlight.color = Color.Lerp(originalSpotLightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);
    }

    //can chase player is spotlight is completely red
    public bool changeToChase()
    {
        if (playerVisibleTimer == timeToSpotPlayer)
        {
            return true;
        }
        else 
            return false;
    }

    //in charge of changing states
    public void TransitionToState(Enemy1BaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") //Enemy damages player when in contact
        {
            Damagable damagableObject = collision.gameObject.GetComponent<Damagable>();
            if (damagableObject != null)
            {
                //Debug.Log("Dealt Damage: " + damage);
                damagableObject.takeHit(damage);
            }
        }
        currentState.OnCollisionEnter(this);
    }

    //checks if enemy1 can see player based on: distance, angle, and obstacles
    public bool canSeePlayer()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < viewDistance) {
            Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
            float angleBetweenEnemy1AndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenEnemy1AndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, playerTransform.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    //checks if there is obstacle between enemy1 and player
    public bool obstacleInFront()
    {
        if (Physics.Linecast(transform.position, playerTransform.position, viewMask))
        {
            return true;
        }
        else
            return false;

    }

    //patrolstate - follows array of waypoints when patrolling
    public IEnumerator FollowPath(Vector3[] waypoints, int startingIndex)
    {
        int targetWayPointIndex = startingIndex;
        Vector3 targetWayPoint = waypoints[targetWayPointIndex];

        while (true)
        {
            //transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);

            //Start pathfinding again if it was stopped
            if (pathfinder.isStopped)
            {
                pathfinder.isStopped = false;
                pathfinder.velocity = unpausedSpeed;
            }
            pathfinder.SetDestination(targetWayPoint);
            unpausedSpeed = pathfinder.velocity;
            
            //if enemy1 arrives at target waypoint, set the destination to the next waypoint
            if (transform.position.x == targetWayPoint.x && transform.position.z == targetWayPoint.z)
            {
                targetWayPointIndex = (targetWayPointIndex + 1) % waypoints.Length;
                targetWayPoint = waypoints[targetWayPointIndex];
                yield return new WaitForSeconds(waitTime); //wait a few seconds
                yield return StartCoroutine(TurnToFace(targetWayPoint)); //turn around towards next waypoint
            }
            yield return null;
            }
    }

    //patrolstate - nested inside FollowPath - turn around towards next waypoint
    public IEnumerator TurnToFace(Vector3 lookTarget)
    {
        //pauses navmesh agent to prevent tracking to waypoint
        pauseNavMesh();

        //finds angle towards next waypoint
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirToLookTarget.x, dirToLookTarget.z) * Mathf.Rad2Deg;

        //changes angle every frame
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;     
            yield return null;
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

    //draws waypoints and ray in front of enemy1 (shows how far it can see) - turn on by clicking Gizmos in Scene window
    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    public override void takeHit(float damage)
    {
        base.takeHit(damage);
        Health.fillAmount = health / startingHealth;
    }
}
