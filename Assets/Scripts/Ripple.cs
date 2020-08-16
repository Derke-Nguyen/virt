using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;
using UnityEngine.Experimental.GlobalIllumination;

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
    public readonly RippleDashState DashState = new RippleDashState();
    public readonly RippleSmashState SmashState = new RippleSmashState();

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

    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;

    List<Vector2> points;
    public NavMeshSurface surface;

    public Pillar pillarPrefab;
    public List<Pillar> pillars;
    //GameObject pill;

    public rippleProjectile projectilePrefab;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool pillarsDone;

    public lightControl lightController;

    public bool isDark;

    public bool endPillar;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        pillars = new List<Pillar>();
        backstabDistance = 11f;
        backstabAngle = 92.3f;
        pillarsDone = false;
        endPillar = false;
        isDark = false;
        pathfinder = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        TransitionToState(FollowState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update(this); //do action based on state
        //surface.BuildNavMesh();
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

    public void turnOffLights()
    {
        lightController.turnLightOff();
        isDark = true;
    }
    public void turnOnLights()
    {
        lightController.turnLightOn();
        isDark = false;
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

    void OnValidate()
    {
        //points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(regionSize / 2, regionSize);
        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                //Vector3 newCenter = new Vector3(points., 0, points.y);
                //Gizmos.DrawSphere(new Vector3(point.x - (50 - 7), 0, point.y - (50 - 7)), displayRadius);
            }
        }
    }

    public IEnumerator SummonPillars()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        foreach (Vector2 point in points)
        {
            Vector3 newPillar = new Vector3(point.x - (50 - 7), -10, point.y - (50 - 7));
            if (newPillar.x >= -7 && newPillar.x <= 7 && newPillar.z >= -6 && newPillar.z <= 7)
            {
                continue;
            }
            Pillar pill = Instantiate(pillarPrefab, new Vector3(point.x - (50 - 7), -10, point.y - (50 - 7)), Quaternion.identity);
            pillars.Add(pill);
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(3f);
        pillarsDone = true;
    }
    public void summonProjectiles()
    {
        for (int i = 0; i < 8; ++i)
        {
            Vector3 dir = Quaternion.Euler(0, i*45, 0) * transform.forward;
            Instantiate(projectilePrefab, (dir - transform.position) * 1.25f, Quaternion.identity);
        }
    }

    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.z - vec1.z, vec2.x - vec1.x);
    }

    //This returns the angle in degrees
    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Abs(AngleInRad(vec1, vec2) * 180 / Mathf.PI);
    }

    public void destroyPillars()
    {
        while (pillars.Count > 0)
        {
            Pillar tempPill = pillars[0];
            pillars.RemoveAt(0);
            tempPill.destroy();
        }
    }

    public IEnumerator findSmashablePillars()
    {
        while (true)
        {
            if (pillars.Count <= 6)
            {
                endPillar = true;
                while(pillars.Count > 0)
                {
                    Pillar tempPill = pillars[0];
                    pillars.RemoveAt(0);
                    tempPill.destroy();
                }
                yield return new WaitForSeconds(5f);
            }
            float minDistance1 = (playerTransform.position - pillars[0].transform.position).magnitude;
            float minDistance2 = (playerTransform.position - pillars[0].transform.position).magnitude;
            int minIndex1 = 0;
            int minIndex2 = 0;
            for (int i = 1; i < pillars.Count; i++)
            {
                if (!pillars[i])
                {
                    if (i + 1 == pillars.Count)
                        break;
                    else
                    {
                        pillars.RemoveAt(i);
                        --i;
                        continue;
                    }
                }
                float distance = (playerTransform.position - pillars[i].transform.position).magnitude;
                if (distance < minDistance1)
                {
                    if (minDistance1 < minDistance2)
                    {
                        minDistance2 = minDistance1;
                        minIndex2 = minIndex1;
                    }
                    minDistance1 = distance;
                    minIndex1 = i;
                }
                else if (distance < minDistance2)
                {
                    if (minDistance2 < minDistance1)
                    {
                        minDistance1 = minDistance2;
                        minIndex1 = minIndex2;
                    }
                    minDistance2 = distance;
                    minIndex2 = i;
                }
            }
            //Instead use raycast to all enemies in range and check if raycasts form 180 degrees maybe
            float targetAngle1 = AngleInDeg(playerTransform.position, pillars[minIndex1].transform.position);
            float targetAngle2 = AngleInDeg(playerTransform.position, pillars[minIndex2].transform.position);
            //Debug.Log("1: " + targetAngle1);
            //Debug.Log("2: " + targetAngle2);
            if (minIndex1 != minIndex2 && Mathf.Abs((180-(targetAngle1 + targetAngle2))) < 15f)
            {
                pillars[minIndex1].activateMove(pillars[minIndex2].transform.position);
                pillars[minIndex2].activateMove(pillars[minIndex1].transform.position);
                Pillar temp = pillars[minIndex2];
                pillars.RemoveAt(minIndex1);
                pillars.Remove(temp);
            }
            yield return null;
        }
    }

}
