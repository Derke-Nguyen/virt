using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;//speed

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
    }
}
