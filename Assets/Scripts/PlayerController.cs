using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 velocity;
    Rigidbody myRigidbody;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        dashTime = startDashTime;
    }

    public void Move(Vector3 _velocity) 
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint)
    {
        
        transform.LookAt(lookPoint);
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if(direction == 0)
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                direction = 1;
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                direction = 2;
            }
            else if(Input.GetKeyDown(KeyCode.W))
            {
                direction = 3;
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                direction = 4;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if(direction == 1)
                {
                    myRigidbody.velocity = Vector2.left * dashSpeed;
                }
            }
        }
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }   
}
