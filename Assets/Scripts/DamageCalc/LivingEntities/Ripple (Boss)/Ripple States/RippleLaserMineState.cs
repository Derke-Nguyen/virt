using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleLaserMineState : RippleBaseState
{
    float angle;
    float speed;
    public override void EnterState(Ripple ripple)
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Blade"));
        ripple.transform.position = new Vector3(0, 2, 45);
        ripple.transform.forward = Vector3.left;
        angle = 0;
        speed = -10f;
        ripple.summonLasers();
        ripple.summonMines();
    }

    public override void FixedStateUpdate(Ripple ripple)
    {

    }

    public override void OnCollisionEnter(Ripple ripple)
    {
        throw new System.NotImplementedException();
    }

    public override void Update(Ripple ripple)
    {
        if (angle >= 180)
        {
            speed *= -1;
            angle = 0;
        }
        ripple.transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.World);
        angle += Time.deltaTime * Mathf.Abs(speed);
    }
}
