using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 8;

    //bullet prefab
    public GameObject smokeBombPrefab;

    PlayerController controller;
    Camera viewCamera;
    

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<PlayerController>();
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        Vector3 velocity = direction * speed;
        controller.Move(velocity);

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }

        //ranged attack if e is pressed
        if (Input.GetKeyDown("e"))
        {
            SmokeBomb();
        }

        
    }

    //spawn a smoke
    void SmokeBomb()
    {
        Vector3 spawnsmokeloc = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1);

        GameObject clone;
        clone = Instantiate(smokeBombPrefab, spawnsmokeloc, Quaternion.identity);
        Destroy(clone, 2.0f);
    }
}
