using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleDashState : RippleBaseState
{
    float speed;
    Vector3 direction;
    Quaternion startingRotation;
    float rotationAmount;
    float startingDistance;
    Vector3 startingPoint;
    float currDistance;
    float spinSpeed;
    public override void EnterState(Ripple ripple)
    {
        startingPoint = ripple.transform.position;
        startingDistance = (ripple.transform.position - ripple.playerTransform.position).magnitude;
        startingRotation = ripple.centralAxis.transform.rotation;
        rotationAmount = 0;
        direction = ripple.transform.forward;
        speed = 50f;
        spinSpeed = 40f;
        ripple.equipWeapon();
    }

    public override void FixedStateUpdate(Ripple ripple)
    {
        
    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        
    }

    public override void Update(Ripple ripple)
    {
        //Debug.Log(Vector3.Distance(ripple.blade.transform.position, ripple.transform.position));
        if ((startingPoint-ripple.transform.position).magnitude <= startingDistance + 10f)
            ripple.transform.Translate(direction * Time.deltaTime * speed, Space.World);
        else
        {
            
            
            if (ripple.blade && Vector3.Distance(ripple.blade.transform.position, ripple.transform.position) >= 3f)
            {
                ripple.blade.transform.Rotate(Vector3.up * Time.deltaTime * 720, Space.World);
                ripple.blade.transform.position = Vector3.MoveTowards(ripple.blade.transform.position, ripple.transform.position, spinSpeed * Time.deltaTime);
            }
            else if (ripple.blade)
            {
                Debug.Log("works");
                GameObject.Destroy(ripple.blade);
            }
            //ripple.blade.transform.Translate(direction * Time.deltaTime * speed, Space.World);
            rotationAmount += Time.deltaTime * 360;
        }
    }
}
