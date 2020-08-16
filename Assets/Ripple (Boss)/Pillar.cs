using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Pillar : MonoBehaviour
{
    float speed;
    float fastSpeed;
    float followSpeed;
    bool move;
    Vector3 destination = Vector3.one;
    float maxTimeSpentInState;

    float damage = 10;

    public LayerMask collisionMask;

    //public NavMeshSurface surface;
    void Start()
    {
        speed = 2;
        fastSpeed = 30;
        followSpeed = 7;
        move = false;
        maxTimeSpentInState = 5f;
        //surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            maxTimeSpentInState -= Time.deltaTime;
            if (maxTimeSpentInState <= 0)
            {
                destroy();
            }
            float moveDistance = followSpeed * Time.deltaTime;
            moveToPlayer(moveDistance);
        }
        else
        {
            //surface.BuildNavMesh();
            if (transform.position.y < -7)
                transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
            else if (transform.position.y < 10)
                transform.Translate(Vector3.up * Time.deltaTime * fastSpeed, Space.World);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pillar")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Player" && move) //Enemy damages player when in contact and in motion
        {
            Damagable damagableObject = collision.gameObject.GetComponent<Damagable>();
            if (damagableObject != null)
            {
                //Debug.Log("Dealt Damage: " + damage);
                damagableObject.takeHit(damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Blade")
        {
            Destroy(this.gameObject);
        }
    }

    public void destroy()
    {
        GameObject.Destroy(gameObject);
    }


    public void activateMove(Vector3 _destination)
    {
        move = true;
        destination = _destination;
    }

    public void moveToPlayer(float moveDistance)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveDistance);
    }
}
