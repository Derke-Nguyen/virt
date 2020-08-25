using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleLaserMineState : RippleBaseState
{
    float angle;
    float speed;
    float numTurns;
    GameObject[] gameObjects;
    public override void EnterState(Ripple ripple)
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Blade"));
        ripple.transform.position = new Vector3(0, 2, 45);
        ripple.transform.rotation = Quaternion.Euler(Vector3.left);
        angle = 0;
        speed = -15f;
        numTurns = 0;
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
        if (numTurns == 1)
        {
            gameObjects = GameObject.FindGameObjectsWithTag("Laser");

            for (var i = 0; i < gameObjects.Length; i++)
            {
                GameObject.Destroy(gameObjects[i]);
            }
            ripple.destroyMines();
            ripple.transform.rotation = Quaternion.Euler(Vector3.forward);
            ripple.transform.forward = Vector3.forward;
            //ripple.weaponHold.position = ripple.vectorDestination(ripple.transform.position, ripple.transform.forward, 5f);
            ripple.centralAxis.rotation = Quaternion.Euler(Vector3.zero);
            ripple.TransitionToState(ripple.FollowState);
        }
        if (angle >= 180)
        {
            speed *= -1;
            angle = 0;
            ++numTurns;
        }
        ripple.transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.World);
        angle += Time.deltaTime * Mathf.Abs(speed);
    }

}
