using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerRangedState : PlayerBaseState
{
    public GameObject bulletprefab;//bulletprefab

    public override void EnterState(PlayerController player)
    {
        bulletprefab = player.GetComponent<PlayerController>().bulletPrefab;//gets prefab from playercontroller

        Vector3 playerPos = player.transform.position;
        Vector3 playerDirection = player.transform.forward;
        Quaternion playerRotation = player.transform.rotation;
        float spawnDistance = 1;

        Vector3 spawnbulletloc = playerPos + playerDirection * spawnDistance;

        GameObject clone = Object.Instantiate(bulletprefab, spawnbulletloc, Quaternion.identity);//creates clone
        clone.GetComponent<Projectile>().Setup(playerDirection);//sends direction of player to Projectile

        player.TransitionToState(player.IdleState);//goes back to idle
    }

    public override void Update(PlayerController player)
    {

    }

    public override void OnCollisionEnter(PlayerController player)
    {

    }

    public override void FixedStateUpdate(PlayerController player)
    {

    }
}
