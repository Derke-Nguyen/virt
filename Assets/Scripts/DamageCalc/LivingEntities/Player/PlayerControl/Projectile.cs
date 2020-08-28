using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;//speed
    float damage = 0.5f;
    float lifespan = 5f;

    private Vector3 direction;//direciton

    public void Setup(Vector3 shootdir)//gets direction from PlayerRangedState
    {
        this.direction = shootdir;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;//moves bullet
        lifespan -= Time.deltaTime;
        if(lifespan <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Damagable>().takeHit(damage);
            GameObject.Destroy(gameObject);
        }
    }
}
