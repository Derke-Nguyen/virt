using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer laser;
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.startWidth = 5f;
        laser.SetWidth(3f, 3f);
        //DrawCircle(this.GetComponent<GameObject>(), 5f, 2f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        laser.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                laser.SetPosition(1, hit.point);
            }
        }
        else
            laser.SetPosition(1, transform.position + (transform.forward * 150));
    }
}
