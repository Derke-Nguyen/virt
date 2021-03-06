﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    PlayerController player;
    public float damage = 10;

    //Allows for access to the player when this script is run
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //Debug.Log("Current Damage: " + damage);
    }

    //If sword collides with enemy, log "Enemy Hit"
    //Only triggers when player is in Melee State (won't trigger from walking into them)
    //TODO: Add velocity when enemy is hit
    //Note: currently logs two hits per swing (one hit for front swing, one hit for backswing)
    //Can change if needed
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyProjectile") && player.CurrentState.Equals(player.MeleeState))
        {
            Damagable damagableObject = other.GetComponent<Damagable>(); //Deals damage by calling takeHit on other object
            if(damagableObject != null)
            {
                //Debug.Log(other.gameObject.name + " was damaged");
                //Debug.Log("Current Damage: " + damage);
                damagableObject.takeHit(damage);
            }
        }
    }

    private void Update()
    {

    }
}
