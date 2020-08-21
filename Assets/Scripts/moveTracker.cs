using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTracker : MonoBehaviour
{
    public LineRenderer laser;
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.useWorldSpace = true;
        laser.startWidth = 5f;
        laser.SetWidth(3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        //laser.SetPosition(0, transform.position);
        //transform.position = transform.parent.position;
        //laser.SetPosition(1, transform.position + (transform.forward * 50));
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.position + (transform.forward * 50));
    }

    public void movePosition(Vector3 destination)
    {
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.position + (transform.forward * 5f * Time.deltaTime));
    }
}
