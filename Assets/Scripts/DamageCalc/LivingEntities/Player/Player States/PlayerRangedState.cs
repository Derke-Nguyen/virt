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

        Vector3 spawnbulletloc = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z + 1);//sets spawn location to player

        GameObject clone = Object.Instantiate(bulletprefab, spawnbulletloc, Quaternion.identity);//creates clone
        clone.GetComponent<Projectile>().Setup(player.transform.forward);//sends direction of player to Projectile

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
