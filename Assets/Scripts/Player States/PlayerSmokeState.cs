using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmokeState : PlayerBaseState
{
    public GameObject SmokePrefab;

    public override void EnterState(PlayerController player)
    {
        SmokePrefab = player.GetComponent<PlayerController>().SmokePrefab;

        Vector3 spawnsmokeloc = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        GameObject clone = Object.Instantiate(SmokePrefab, spawnsmokeloc, Quaternion.identity);
        Object.Destroy(clone, 2.0f);
        player.TransitionToState(player.IdleState);
    }

    //Nothing should go here
    public override void FixedStateUpdate(PlayerController player)
    {

    }

    public override void OnCollisionEnter(PlayerController player)
    {

    }

    // Update is called once per frame
    public override void Update(PlayerController player)
    {

    }
}
