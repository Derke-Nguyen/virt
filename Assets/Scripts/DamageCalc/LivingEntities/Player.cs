﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public float speed = 8;

    public float invincibility = 0; //Tracks if the player has mercy invincibility
    float invincibilityTime = 1.2f;
    int flickerTimes = 5;
    float flickOn = 0.2f;
    float flickOff = 0.2f; //Time spent on player object flicker

    PlayerController controller; //Reference to player's transform
    Camera viewCamera; //Reference to main camera

    public TimeManager timeManager;

    public Image Health; //Reference to image for healthbar

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        controller = FindObjectOfType<PlayerController>();
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    //Each frame, recalculate direction and velocity based on input
    //Note: might be very computationally heavy
    //Possible solution: make a coroutine that doesn't trigger every frame (refresh rate > 1 frame)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            timeManager.bulletTime();
        }
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        Vector3 velocity = direction * speed;
        controller.Move(velocity);
        controller.setDirection(direction);
    }

    //Points player at mouse
    private void FixedUpdate()
    {
        if (!controller.CurrentState.Equals(controller.MeleeState))
        {
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                //Debug.DrawLine(ray.origin, point, Color.red);
                controller.LookAt(point);
            }
        }
        if(invincibility > 0)
        {
            invincibility -= Time.deltaTime;
        }
        if(invincibility < 0)
        {
            invincibility = 0;
            StopCoroutine(Blink(flickerTimes, flickOn, flickOff)); //Ends flickering when invincibility ends
        }
    }

    //Used to update healthbar in real time
    public override void takeHit(float damage)
    {
        if(invincibility > 0)
        {
            //Debug.Log(invincibility);
            return;
        }
        base.takeHit(damage);
        Health.fillAmount = health / startingHealth; //Changes proportion of healthbar that is green
        invincibility = invincibilityTime;
        StartCoroutine(Blink(flickerTimes, flickOn, flickOff)); //Start flicker Coroutine
    }

    //Coroutine to cause player to flicker when damaged
    IEnumerator Blink(int nTimes, float timeOn, float timeOff)
    {
        while (nTimes > 0)
        {
            this.transform.Find("player_model").gameObject.SetActive(false);
            yield return new WaitForSeconds(timeOn);
            this.transform.Find("player_model").gameObject.SetActive(true);
            yield return new WaitForSeconds(timeOff);
            nTimes--;
        }
        GetComponent<Renderer>().enabled = true;
    }
}
