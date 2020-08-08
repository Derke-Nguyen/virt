using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmokeState : PlayerBaseState
{
    //smokeprefab
    public GameObject SmokePrefab;

    public override void EnterState(PlayerController player)
    {
        //gets smokeprefab from playercontroller
        SmokePrefab = player.GetComponent<PlayerController>().SmokePrefab;

        //gets position of player
        Vector3 spawnsmokeloc = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        //clones prefab and instatiates it
        GameObject clone = Object.Instantiate(SmokePrefab, spawnsmokeloc, Quaternion.identity);
        Object.Destroy(clone, 2.0f);//destorys it
        player.TransitionToState(player.IdleState);//goes back to idle
    }

    //Nothing goes here
    public override void FixedStateUpdate(PlayerController player)
    {

    }

    //pretty sure nothing goes here
    public override void OnCollisionEnter(PlayerController player)
    {

    }

    // Update is called once per frame
    public override void Update(PlayerController player)
    {

    }
}
