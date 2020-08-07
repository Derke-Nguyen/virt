using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public float speed = 8;

    PlayerController controller; //Reference to player's transform
    Camera viewCamera; //Reference to main camera

    public Image Health; //Reference to image for healthbar

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        controller = FindObjectOfType<PlayerController>();
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    //Each frame, recalculate direction and velocity based on input
    //Note: might be very computationally heavy
    //Possible solution: make a coroutine that doesn't trigger every frame (refresh rate > 1 frame)
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        Vector3 velocity = direction * speed;
        controller.Move(velocity);
        controller.setDirection(direction);
    }

    //Points player at mouse
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

    //Used to update healthbar in real time
    public override void takeHit(float damage)
    {
        base.takeHit(damage);
        Health.fillAmount = health / startingHealth;
    }
}
