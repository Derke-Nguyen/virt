using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Enemy
{
    public float lightRatio;
    float timeToSpotPlayer = 0.5f;
    public float playerVisibleTimer;
    public Light spotlight;
    Color originalSpotLightColor;
    public Transform playerTransform;
    public float viewDistance;
    public LayerMask viewMask;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        lightRatio = 0;
        originalSpotLightColor = spotlight.color;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        viewDistance = 7f;  
    }

    // Update is called once per frame
    void Update()
    {
        if (lightRatio == 1)
        {
            explode();
        }
        lightControl();
        inTheRed();
    }

    public void explode()
    {
        //TODO
    }

    public void destroyMine()
    {
        Destroy(this.gameObject);
    }
    public override bool inTheRed()
    {
        lightRatio = (playerVisibleTimer / timeToSpotPlayer);
        //based on how red spotlight is, return true
        if (lightRatio > 0.50)
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
            spotlight.enabled = true;
            playerVisibleTimer += Time.deltaTime/2;
        }
        else
        {
            spotlight.enabled = false;
            playerVisibleTimer -= Time.deltaTime;
        }
        //clamps value to timeToSpotPlayer
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        //changes color gradually based on a fractional value of playerVisibleTimer / timeToSpotPlayer
        spotlight.color = Color.Lerp(originalSpotLightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);
    }

    public bool canSeePlayer()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < viewDistance)
        {
            if (!Physics.Linecast(transform.position, playerTransform.position, viewMask)) 
            {
                return true;
            }
        }
        return false;
    }
}
