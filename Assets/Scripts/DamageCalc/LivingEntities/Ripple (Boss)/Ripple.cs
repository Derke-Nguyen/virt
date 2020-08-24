﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI; //Image
using UnityEngine;
using System;
using UnityEngine.Experimental.GlobalIllumination;

public class Ripple : Enemy
{
    private RippleBaseState currentState;
    public RippleBaseState CurrentState
    {
        get { return currentState; }
    }


    //DAMAGE CALCULATION
    //NOTE: STARTING HEALTH SHOULD BE SET FROM UNITY
    public Image Health; //Health Bar Renderer
    float damage = 10; //contact damage from Ripple

    //TODO: SWTICH TO MORE EFFECTIVE FORM OF DAMAGE HANDLING

    //END DAMAGE CALCULATION

    public readonly RippleFollowState FollowState = new RippleFollowState();
    public readonly RippleSwingState SwingState = new RippleSwingState();
    public readonly RippleSummonState SummonState = new RippleSummonState();
    public readonly RippleDarkKnivesState DarkKnivesState = new RippleDarkKnivesState();
    public readonly RippleAssassinState AssassinState = new RippleAssassinState();
    public readonly RippleDodgeState DodgeState = new RippleDodgeState();
    public readonly RippleDashState DashState = new RippleDashState();
    public readonly RippleSmashState SmashState = new RippleSmashState();
    public readonly RippleTeleportState TeleportState = new RippleTeleportState();
    public readonly RippleWideSwingState WideSwingState = new RippleWideSwingState();
    public readonly RippleLaserMineState LaserMineState = new RippleLaserMineState();
    public readonly RippleDarkChaseState DarkChaseState = new RippleDarkChaseState();

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
    public List<Pillar> eightPillars;
    public List<rippleProjectile> projectiles;
    public List<Laser> lasers;

    public rippleProjectile projectilePrefab;
    public Laser laserPrefab;
    public Mine minePrefab;
    public LightBlade lightBladePrefab;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool pillarsDone;

    public lightControl lightController;

    public Light spotlight;
    Color originalSpotLightColor;

    public float lightViewAngle;

    public bool isDark;

    public bool endPillar;

    float timeToSpotPlayer = 0.5f;
    public float playerVisibleTimer;

    public float viewDistance;

    public moveTracker tracker;

    public bool pausedState;

    public RippleBaseState previousState;

    public float lightRatio;

    public bool lightBladeActivated;
    
    //float size;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        pillars = new List<Pillar>();
        eightPillars = new List<Pillar>();
        projectiles = new List<rippleProjectile>();
        lasers = new List<Laser>();
        backstabDistance = 11f;
        backstabAngle = 92.3f;
        pillarsDone = false;
        endPillar = false;
        isDark = false;
        pausedState = false;
        lightBladeActivated = false;
        pathfinder = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        viewDistance = 15f;
        lightRatio = 0;
        lightViewAngle = spotlight.spotAngle;
        originalSpotLightColor = spotlight.color;

        //size = 5f;
        
        
        //TODO Change starting state
        currentState = FollowState;
        TransitionToState(FollowState);
    }

    // Update is called once per frame
    void Update()
    {
        //size += 0.1f;
        //var go1 = new GameObject { name = "Circle" };
        //go1.DrawCircle(size, 1f);
        //go1.DrawCircle(size, 1f);
        if (!pausedState)
        {
            currentState.Update(this); //do action based on state
        }
        if (isDark)
        {
            if ((playerTransform.position - transform.position).magnitude <= 10f)
            {
                transform.localScale = new Vector3(2, 2, 2);
            }
        }
        else
        {
            if (currentState != WideSwingState)
                lightControl();
            else
                forcedLightControl(1f);
        }

        inTheRed(); //checks if spotlight is red
    }

    public void FixedUpdate()
    {
        if (!pausedState)
        {
            currentState.FixedStateUpdate(this); //do action based on state
        }
    }

    public void TransitionToState(RippleBaseState state)
    {
        previousState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }

    public void deactivateLight()
    {
        spotlight.enabled = false;
    }

    public void activateLight()
    {
        spotlight.enabled = true;
    }

    public override bool inTheRed()
    {
        lightRatio = (playerVisibleTimer / timeToSpotPlayer);
        //based on how red spotlight is, return true
        if (lightRatio > 0.75)
        {
            return true;
        }
        else
            return false;
    }
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

    public void forcedLightControl(float lightSpeed)
    {
        playerVisibleTimer += Time.deltaTime * lightSpeed;
        //clamps value to timeToSpotPlayer
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        //changes color gradually based on a fractional value of playerVisibleTimer / timeToSpotPlayer
        spotlight.color = Color.Lerp(originalSpotLightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);
    }
    public bool canSeePlayer()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < viewDistance)
        {
            Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
            float angleBetweenEnemy1AndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenEnemy1AndPlayer < lightViewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, playerTransform.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
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

    public void summon8Pillars()
    {
        if (eightPillars.Count > 0)
        {
            return;
        }
        eightPillars.Add(Instantiate(pillarPrefab, new Vector3(-35f,0, 25f), Quaternion.identity));
        eightPillars.Add(Instantiate(pillarPrefab, new Vector3(-35f, 0, 0f), Quaternion.identity));
        eightPillars.Add(Instantiate(pillarPrefab, new Vector3(-35f, 0, -25f), Quaternion.identity));
        eightPillars.Add(Instantiate(pillarPrefab, new Vector3(0f, 0, 25f), Quaternion.identity));
        eightPillars.Add(Instantiate(pillarPrefab, new Vector3(0f, 0, -25f), Quaternion.identity));
        eightPillars.Add(Instantiate(pillarPrefab, new Vector3(35f, 0, 25f), Quaternion.identity));
        eightPillars.Add(Instantiate(pillarPrefab, new Vector3(35f, 0, 0f), Quaternion.identity));
        eightPillars.Add(Instantiate(pillarPrefab, new Vector3(35f, 0, -25f), Quaternion.identity));
    }

    public void summonMines()
    {
        points = PoissonDiscSampling.GeneratePoints(10f, regionSize, rejectionSamples);
        foreach (Vector2 point in points)
        {
            //Vector3 newMine = new Vector3(point.x - (50 - 7), -10, point.y - (50 - 7));
            Instantiate(minePrefab, new Vector3(point.x - (50 - 7), 1, point.y - (50 - 7+5)), Quaternion.identity);
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
        yield return new WaitForSeconds(2.5f);
        pillarsDone = true;
    }

    public IEnumerator PauseState(float seconds)
    {
        pausedState = true;
        yield return new WaitForSeconds(seconds);
        pausedState = false;
    }
    public void summonProjectiles()
    {
        projectiles.Clear();
        for (int i = 0; i < 8; ++i)
        {
            Vector3 dir = Quaternion.Euler(0, i*45, 0) * transform.forward;
            rippleProjectile proj = Instantiate(projectilePrefab, (dir - transform.position) * 1.25f, Quaternion.identity);
            projectiles.Add(proj);
        }
    }
    public void summonLasers()
    {
        lasers.Clear();
        for (int i = 0; i < 3; ++i)
        {
            Laser ls = Instantiate(laserPrefab, transform.position, Quaternion.Euler(0, -55 - (i*55), 0), centralAxis);
            ls.transform.parent = centralAxis;
            lasers.Add(ls);
        }
    }
    public bool checkIfNoProjectiles()
    {
        if (GameObject.FindGameObjectWithTag("EnemyProjectile") == null)
        {
            return true;
        }
        else
            return false;
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
    public IEnumerator summonLightBlades()
    {
        lightBladeActivated = true;
        for (int i = 0; i < 15; ++i)
        {
            Vector3 destination = vectorDestination(playerTransform.position, Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0)*transform.forward, 10f);
            Instantiate(lightBladePrefab, destination, Quaternion.Euler(90,0,0));
            yield return new WaitForSeconds(0.8f);
        }
        lightBladeActivated = false;
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

    //when finding vector3 desintation on a line with two Vector3 points
    public Vector3 vectorDestination(Vector3 origin, Vector3 _direction, float distance)
    {
        Vector2 direction = new Vector2(_direction.x, _direction.z);
        Vector2 unitVector;
        if (direction.magnitude == 0)
        {
            unitVector = direction;
        }
        else
        {
            unitVector = direction / (direction.magnitude);
        }
        unitVector *= distance;
        return (new Vector3 (origin.x + unitVector.x, origin.y, origin.z + unitVector.y));
    }

    //DAMAGE CALCULATIONS
    public override void takeHit(float damage)
    {
        if ((currentState != SmashState && currentState != SummonState) || (currentState == SummonState && isDark == true))
        {
            base.takeHit(damage);
            Health.fillAmount = health / startingHealth;
        }
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
    }
}
