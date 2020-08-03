using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 velocity;
    Rigidbody myRigidbody;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
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
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }   
}
