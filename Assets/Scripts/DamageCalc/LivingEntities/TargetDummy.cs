﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TargetDummy : LivingEntity
{
    Transform player; //Reference to Player
    NavMeshAgent agent; //Navigation AI

    public Image Health; //Health Bar Renderer

    float damage = 5; //Damage per hit

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); //Calls the start of Living Entity
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    public override void takeHit(float damage)
    {
        base.takeHit(damage); //Calls the takeHit of Living Entity
        Health.fillAmount = health / startingHealth; //Decides how much of the healthbar to fill in
    }

    // Update is called once per frame
    void Update()
    {
        if((player.position - transform.position).magnitude < 15) //Starts coroutine if player is within 15 units
        {
            StartCoroutine(UpdatePath());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    IEnumerator UpdatePath() //Coroutine for chasing player
    {
        //Debug.Log("We got here");
        float refresh = 1;

        while(player != null)
        {
            Vector3 targetPosition = player.position;
            agent.SetDestination(targetPosition);
            yield return new WaitForSeconds(refresh);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //Enemy damages player when in contact
        {
            Damagable damagableObject = other.GetComponent<Damagable>();
            if (damagableObject != null)
            {
                //Debug.Log("Dealt Damage: " + damage);
                damagableObject.takeHit(damage);
            }
        }
    }
}
