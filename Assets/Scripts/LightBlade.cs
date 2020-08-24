using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlade : MonoBehaviour
{
    float rotationAmount;
    Vector3 direction;
    public Transform playerTransform;
    bool moveForward;
    // Start is called before the first frame update
    void Start()
    {
        rotationAmount = 0;
        moveForward = false;
        direction = transform.forward;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveForward)
        {
            transform.Translate(transform.up * Time.deltaTime * 35f, Space.World);
            //if ((playerTransform.position - transform.position).magnitude >= 20f)
            //{
            //   Destroy(this.gameObject);
            //}
        }
        if (!moveForward && rotationAmount >= 180)
        {
            Vector3 dirToPlayer = playerTransform.position - transform.position;
            float angle = Vector3.Angle(transform.up, dirToPlayer);
            if (angle < 10f)
            {
                moveForward = true;
            }
            else
            {
                transform.Rotate(Vector3.up * Time.deltaTime * -360, Space.World);
                rotationAmount += Time.deltaTime * 360;
            }
        }
        else if (!moveForward)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * -360, Space.World);
            rotationAmount += Time.deltaTime * 360;
        }
    }
}
