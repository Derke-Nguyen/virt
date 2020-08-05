using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy"
            && player.CurrentState.Equals(player.MeleeState))
        {
            Debug.Log("Enemy Hit");
        }
    }

    private void Update()
    {

    }
}
