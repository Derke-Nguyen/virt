using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarDetecter : MonoBehaviour
{
    LineRenderer line;
    float lineWidth;
    float radius;
    public Pillar prefabPillar;
    Pillar pill;
    bool waitForPillar;
    bool hasSummoned;
    // Start is called before the first frame update
    void Start()
    {
        waitForPillar = false;
        hasSummoned = false;
        radius = 0f;
        var segments = 360;
        lineWidth = 0.3f;
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
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 1, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasSummoned && !pill)
        {
            //waitForPillar = false;
        }
        
        if (!waitForPillar)
        {
            if (radius <= 5 )
            {
                updateCircle();
            }
            else
            {
                line.enabled = false;
                waitForPillar = true;
                StartCoroutine(SummonPillars());
            }
        }
    }

    void updateCircle()
    {
        radius += 0.02f;
        var segments = 360;
        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 1, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }

    public IEnumerator SummonPillars()
    {
        pill = Instantiate(prefabPillar, new Vector3(transform.position.x, -3, transform.position.z), Quaternion.identity, transform);
        yield return new WaitForSeconds(11f);
        radius = 0;
        hasSummoned = true;
        line.enabled = true;
        waitForPillar = false;
    }
}
