using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public bool isOff;
    void Start()
    {
        isOff = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOff)
        {
            this.GetComponent<TrailRenderer>().enabled = false;
        }
        else
            this.GetComponent<TrailRenderer>().enabled = true;
    }

    public void turnTrailOff()
    {
        isOff = true;
    }

    public void turnTrailOn()
    {
        isOff = false;
    }
}
