using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mineLight : MonoBehaviour
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
            this.GetComponent<Light>().enabled = false;
        }
        else
            this.GetComponent<Light>().enabled = true;
    }

    public void turnLightOff()
    {
        isOff = true;
    }

    public void turnLightOn()
    {
        isOff = false;
    }
}
