using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    PlayerController player;

    //Allows for access to the player when this script is run
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    //If sword collides with enemy, log "Enemy Hit"
    //Only triggers when player is in Melee State (won't trigger from walking into them)
    //TODO: Add velocity when enemy is hit
    //Note: currently logs two hits per swing (one hit for front swing, one hit for backswing)
    //Can change if needed
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy"
            && player.CurrentState.Equals(player.MeleeState))
        {
            //Debug.Log("Enemy Hit");
        }
    }

    private void Update()
    {

    }
}
