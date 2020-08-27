using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float damage = 20;

    LineRenderer laser;
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
        laser.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                laser.SetPosition(1, hit.point);
                hit.collider.GetComponent<Player>().takeHit(damage);
            }
        }
        else
        {
            laser.SetPosition(1, transform.position + (transform.forward * 100));
        }
    }
}