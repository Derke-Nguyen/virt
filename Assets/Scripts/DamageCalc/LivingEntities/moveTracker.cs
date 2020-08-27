using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTracker : MonoBehaviour
{
    public LineRenderer tracker;
    // Start is called before the first frame update
    void Start()
    {
        tracker = GetComponent<LineRenderer>();
        tracker.useWorldSpace = true;
        tracker.startWidth = 5f;
        tracker.endWidth = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        //laser.SetPosition(0, transform.position);
        //transform.position = transform.parent.position;
        //laser.SetPosition(1, transform.position + (transform.forward * 50));
        //tracker.SetPosition(0, transform.position);
        //tracker.SetPosition(1, transform.position + (transform.forward * 50));
    }

    public void movePosition(Vector3 destination)
    {
        tracker.SetPosition(0, transform.position);
        tracker.SetPosition(1, destination);
    }

    public void turnTrackerOff()
    {
        tracker.enabled = false;
    }

    public void turnTrackerOn()
    {
        tracker.enabled = true;
    }
}
