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
<<<<<<< Updated upstream
    Rigidbody myRigidbody;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
=======
    
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;
    public float jumpForce = 8;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        dashTime = startDashTime;

        TransitionToState(IdleState);
    }

    private void Update()
    {
        currentState.Update(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this);
    }

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
=======
        if(direction == 0)
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                direction = 1;
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                direction = 2;
            }
            else if(Input.GetKeyDown(KeyCode.W))
            {
                direction = 3;
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                direction = 4;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if(direction == 1)
                {
                    playerRigidBody.velocity = Vector2.left * dashSpeed;
                }
            }
        }
        playerRigidBody.MovePosition(playerRigidBody.position + velocity * Time.fixedDeltaTime);
>>>>>>> Stashed changes
    }   
}
