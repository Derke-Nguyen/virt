using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Pillar : MonoBehaviour
{
    //float speed;
    //float amount;
    //float followSpeed;
    //float maxTimeSpentInState;
    float fastSpeed;
    Vector3 destination = Vector3.one;
    

    public LayerMask collisionMask;

    bool pausedState;

    bool goingToGround;

    //public NavMeshSurface surface;
    void Start()
    {
        //speed = 2;
        //amount = 1f;
        //followSpeed = 7;
        //maxTimeSpentInState = 5f;


        fastSpeed = 3f;
        pausedState = false;
        goingToGround = false;
        //surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= 1 && !pausedState)
        {
            StartCoroutine(PauseState(7f));
        }
        if (goingToGround)
        {
            transform.Translate(Vector3.up * Time.deltaTime * -fastSpeed, Space.World);
            if (transform.position.y <= -3)
            {
                Destroy(this.gameObject);
            }
        }
        else if (transform.position.y < 1)
        {
            transform.Translate(Vector3.up * Time.deltaTime * fastSpeed, Space.World);
        }
        
    }

    public void destroyPillar()
    {
        goingToGround = true;
    }

    void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Blade" || other.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }

    public void destroy()
    {
        GameObject.Destroy(gameObject);
    }

    public IEnumerator PauseState(float seconds)
    {
        pausedState = true;
        yield return new WaitForSeconds(seconds);
        pausedState = false;
        goingToGround = true;
    }


}
