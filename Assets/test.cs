using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    LineRenderer line;
    float lineWidth;
    float radius;
    // Start is called before the first frame update
    void Start()
    {
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
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 1, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }

    // Update is called once per frame
    void Update()
    {
        radius += 0.1f;
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
}
