using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update



    Rigidbody playerRigidBody;
    public Rigidbody Rigidbody
    {
        get { return playerRigidBody; }
    }

    private PlayerBaseState currentState;
    public PlayerBaseState CurrentState
    {
        get { return currentState; }
    }

    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerDashingState DashingState = new PlayerDashingState();

    Vector3 velocity;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private Vector3 direction;
    public float jumpForce = 8;
    private bool dashState = false;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        dashTime = startDashTime;

        TransitionToState(IdleState);
    }

    private void Update()
    {
        currentState.Update(this);
        float inputAxisX = Input.GetAxis("Vertical");
        float inputAxisY = Input.GetAxis("Horizontal");
        Vector3 directionVector = new Vector3(-inputAxisX, 0, inputAxisY);
        directionVector.Normalize();
        direction = directionVector;
        if (directionVector == Vector3.zero)
        {
            //Do if user inputs nothing;

        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashState = true;
            dashTime = startDashTime;
        }
        if (dashState)
        {
            transform.Translate(directionVector * dashSpeed * Time.deltaTime);
            dashTime -= Time.deltaTime;
            if(dashTime <= 0)
            {
                dashState = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this);
    }

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void Move(Vector3 _velocity) 
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint)
    {
        transform.LookAt(lookPoint);
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!dashState)
        {
            Rigidbody.MovePosition(Rigidbody.position + velocity * Time.fixedDeltaTime);
        }
    } 
}
