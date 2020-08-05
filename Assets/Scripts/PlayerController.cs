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
    public readonly PlayerMeleeState MeleeState = new PlayerMeleeState();


    public Vector3 velocity;
    
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    public Vector3 direction;

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
        direction = transform.forward;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!CurrentState.Equals(DashingState))
        {
            Rigidbody.MovePosition(Rigidbody.position + velocity * Time.fixedDeltaTime);
        }
        currentState.FixedStateUpdate(this);
    }   

}
