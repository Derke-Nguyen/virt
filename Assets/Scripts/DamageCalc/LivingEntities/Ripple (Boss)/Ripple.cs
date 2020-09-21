using System.Collections;
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
    public Slider Health; //Health Bar Renderer
    float damage = 10; //contact damage from Ripple

    //TODO: SWTICH TO MORE EFFECTIVE FORM OF DAMAGE HANDLING

    //END DAMAGE CALCULATION

    //Finite State Machines
    public readonly RippleFollowState FollowState = new RippleFollowState();
    public readonly RippleSwingState SwingState = new RippleSwingState();
    public readonly RippleSummonState SummonState = new RippleSummonState();
    public readonly RippleDodgeState DodgeState = new RippleDodgeState();
    public readonly RippleDashState DashState = new RippleDashState();
    public readonly RippleTeleportState TeleportState = new RippleTeleportState();
    public readonly RippleWideSwingState WideSwingState = new RippleWideSwingState();
    public readonly RippleLaserMineState LaserMineState = new RippleLaserMineState();
    public readonly RippleDarkChaseState DarkChaseState = new RippleDarkChaseState();

    //Setup Navmesh
    public NavMeshAgent pathfinder;
    public Transform playerTransform;
    public Vector3 unpausedSpeed = Vector3.zero;

    //Ripple weapon variables
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
    public List<pillarDetecter> pillars;
    public List<Laser> lasers;
    public List<ShockWave> shockwaves;
    public List<Mine> mines;

    public Laser laserPrefab;
    public Mine minePrefab;
    public LightBlade lightBladePrefab;
    public pillarDetecter pillarDetecterPrefab;
    public ShockWave shockWavePrefab;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

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

    public bool noSpotLight;
    
    //float size;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        pillars = new List<pillarDetecter>();
        lasers = new List<Laser>();
        shockwaves = new List<ShockWave>();
        mines = new List<Mine>();
        backstabDistance = 11f;
        backstabAngle = 92.3f;
        endPillar = false;
        isDark = false;
        pausedState = false;
        lightBladeActivated = false;
        noSpotLight = false;
        pathfinder = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        viewDistance = 15f;
        lightRatio = 0;
        lightViewAngle = spotlight.spotAngle;
        originalSpotLightColor = spotlight.color;

        currentState = FollowState;
        TransitionToState(FollowState);
    }

    // Update is called once per frame
    void Update()
    {
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
            if (currentState != WideSwingState && !noSpotLight)
                lightControl();
            else if (!noSpotLight)
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
    //Used for finite state machine transitions
    public void TransitionToState(RippleBaseState state)
    {
        previousState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }
    //Turns off spotlight
    public void deactivateLight()
    {
        noSpotLight = true;
        lightRatio = 0;
        playerVisibleTimer = 0;
        spotlight.enabled = false;
    }
    //Turns on spotlight
    public void activateLight()
    {
        noSpotLight = false;
        spotlight.enabled = true;
    }
    //Returns true if range indicator is red
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
    //Uses color to show how long Player was in range of Ripple
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
    //Turns off automatic light control and has lightControl on all the time
    public void forcedLightControl(float lightSpeed)
    {
        playerVisibleTimer += Time.deltaTime * lightSpeed;
        //clamps value to timeToSpotPlayer
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        //changes color gradually based on a fractional value of playerVisibleTimer / timeToSpotPlayer
        spotlight.color = Color.Lerp(originalSpotLightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);
    }
    //checks if Player is in range of Ripple
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
    //Instantiates Ripple's weapon
    public void equipWeapon()
    {
        if (!blade)
        {
            blade = Instantiate(equippedBlade, weaponHold.position, weaponHold.rotation);
            blade.transform.parent = weaponHold;
        }

    }
    //Turns off directional light
    public void turnOffLights()
    {
        lightController.turnLightOff();
        isDark = true;
    }
    //Turns on directional light
    public void turnOnLights()
    {
        lightController.turnLightOn();
        isDark = false;
    }
    //Pauses NavmeshAgent
    public void pauseNavMesh()
    {
        pathfinder.velocity = Vector3.zero;
        pathfinder.isStopped = true;
    }

    //Pathfinds towards player every 0.1s
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
    //Instantiates the Mines
    public void summonMines()
    {
        mines.Clear();
        points = PoissonDiscSampling.GeneratePoints(10f, regionSize, rejectionSamples);
        foreach (Vector2 point in points)
        {
            mines.Add(Instantiate(minePrefab, new Vector3(point.x - (70 - 7), 1, point.y - (50 - 7+5)), Quaternion.identity));
        }
    }
    //Destroys the Mines
    public void destroyMines()
    {
        foreach (Mine mine in mines)
        {
            if (mine)
            {
                mine.destroyMine();
            }
        }
    }
    //Instantiates Pillars using PoissonDiscSampling
    public IEnumerator SummonPillars()
    {
        pillars.Clear();
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        foreach (Vector2 point in points)
        {
            pillarDetecter pill = Instantiate(pillarDetecterPrefab, new Vector3(point.x - (70 - 7), 0, point.y - (50 - 7)), Quaternion.identity);
            pillars.Add(pill);
            yield return new WaitForSeconds(0.7f);
        }
        yield return new WaitForSeconds(2.5f);
    }
    //Pauses Ripple for x seconds
    public IEnumerator PauseState(float seconds)
    {
        pausedState = true;
        yield return new WaitForSeconds(seconds);
        pausedState = false;
    }
    //Instantiates three lasers
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
    //checks if there are no projectiles
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

    //Destroys Pillars
    public void destroyPillars()
    {
        while (pillars.Count > 0)
        {
            pillarDetecter tempPill = pillars[0];
            pillars.RemoveAt(0);
            tempPill.destroyPillarDetecter();
        }
    }

    //Instantiates LightBlades near Player
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

    //Instantiates Shockwaves on four corners
    public void summonShockWaves()
    {
        shockwaves.Clear();
        shockwaves.Add(Instantiate(shockWavePrefab, new Vector3(-66, 1, 45), Quaternion.identity));
        shockwaves.Add(Instantiate(shockWavePrefab, new Vector3(-66, 1, -45), Quaternion.identity));
        shockwaves.Add(Instantiate(shockWavePrefab, new Vector3(66, 1, 45), Quaternion.identity));
        shockwaves.Add(Instantiate(shockWavePrefab, new Vector3(66, 1, -45), Quaternion.identity));
    }

    //checks if there are no ShockWaves
    public bool noShockwaves()
    {
        for (int i = 0; i < shockwaves.Count; ++i)
        {
            if (shockwaves[i])
            {
                return false;
            }
        }
        return true;
    }


    //Finds Vector3 destination using a line made with two Vector3 points and the distance past the second point
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
        if ((currentState != SummonState) || (currentState == SummonState && isDark == true))
        {
            base.takeHit(damage);
            Health.value = health / startingHealth;
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
