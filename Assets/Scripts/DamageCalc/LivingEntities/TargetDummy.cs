using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TargetDummy : LivingEntity
{
    Transform player;
    NavMeshAgent agent;

    public Image Health;

    float damage = 5;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    public override void takeHit(float damage)
    {
        base.takeHit(damage);
        Health.fillAmount = health / startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if((player.position - transform.position).magnitude < 15)
        {
            StartCoroutine(UpdatePath());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    IEnumerator UpdatePath()
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
        if (other.gameObject.tag == "Player")
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
