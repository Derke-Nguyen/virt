using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : Enemy
{
    LineRenderer line;
    float lineWidth;
    float radius;

    public Transform playerTransform;
    public Light spotlight;
    Color originalSpotLightColor;

    float lightRatio;
    float timeToSpotPlayer = 1f;
    float playerVisibleTimer;
    float viewDistance;

    public LayerMask viewMask;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        lightRatio = 0;
        originalSpotLightColor = spotlight.color;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        viewDistance = 7f;

        radius = 1f;
        var segments = 360;
        lineWidth = 1f;
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0.5f, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }

    // Update is called once per frame
    void Update()
    {
        if (lightRatio == 1)
        {
            radius += 0.5f;
        }
        lightControl();
        inTheRed();
        if (radius >= 125f)
        {
            radius = 0;
        }
        radius += 0.03f;
        var segments = 360;
        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0.5f, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
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
            playerVisibleTimer += Time.deltaTime / 2;
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

    public override void takeHit(float damage)
    {
        base.takeHit(damage);
        Debug.Log(health);
    }
}
