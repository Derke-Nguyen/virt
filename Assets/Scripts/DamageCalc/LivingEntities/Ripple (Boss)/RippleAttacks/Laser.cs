using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float damage = 20;

    LineRenderer laser;
    public Vector3 offset = new Vector3(0, 1f, 0);
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.startWidth = 3f;
        laser.endWidth = 3f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        laser.SetPosition(0, transform.position - offset);
        RaycastHit hit;
        if (Physics.Raycast(transform.position - offset, transform.forward, out hit))
        {
            if (hit.collider.GetComponent<Player>())
            {
                //Debug.Log("WE GOT HERE");
                hit.collider.GetComponent<Player>().takeHit(damage);
            }
        }
        else
        {
            laser.SetPosition(1, transform.position - offset + (transform.forward * 150));
        }
    }
}