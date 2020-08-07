using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public float speed = 8;

    PlayerController controller;
    Camera viewCamera;

    public Image Health;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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


    }

    private void FixedUpdate()
    {
        if (!controller.CurrentState.Equals(controller.MeleeState))
        {
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                //Debug.DrawLine(ray.origin, point, Color.red);
                controller.LookAt(point);
            }
        }
    }

    public override void takeHit(float damage)
    {
        base.takeHit(damage);
        Health.fillAmount = health / startingHealth;
    }
}
