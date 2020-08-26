using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    public readonly PlayerSmokeState SmokeState = new PlayerSmokeState();
    public readonly PlayerRangedState RangedState = new PlayerRangedState();
    public readonly PlayerAssassinState AssassinState = new PlayerAssassinState();

    public GameObject SmokePrefab;

    public GameObject bulletPrefab;

    public Vector3 velocity;
    
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    public Vector3 direction;

    public float jumpForce = 8;

    GameObject[] enemies;
    float minDistance;
    int index;
    public bool canAssassinate;
    public Transform targetTransform;
    GameObject target;
    public TimeManager timeManager;

    public Transform CentralAxis;

    public TrailController trailController;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        dashTime = startDashTime;
        canAssassinate = false;

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
        //direction = transform.forward;
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

    //finds nearest enemy that the player can assassinate (requires all enemies to have inTheRed function)	
    public void findNearestEnemy(Vector3 mousePoint)
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        minDistance = 1000;
        index = -1;
        for (int i = 0; i < enemies.Length; ++i)
        {
            if (enemies[i].GetComponent<Enemy>().inTheRed())
            {
                if ((enemies[i].transform.position - mousePoint).magnitude < minDistance)
                {
                    minDistance = (enemies[i].transform.position - transform.position).magnitude;
                    index = i;
                }
            }
        }
        if (minDistance <= 1000f && index != -1)
        {
            canAssassinate = true;
            target = enemies[index];
            targetTransform = enemies[index].transform;
        }
        else
        {
            canAssassinate = false;
        }
    }
    public void assassinate()
    {
        //timeManager.bulletTime();	
        Vector3 dir = (targetTransform.position - transform.position).normalized;
        transform.position = vectorDestination(targetTransform.position, dir, 5f);
        //GameObject.Destroy(target);
    }

    public Vector3 vectorDestination(Vector3 origin, Vector3 _direction, float distance)
    {
        Vector2 direction = new Vector2(_direction.x, _direction.z);
        Vector2 unitVector;
        if (direction.magnitude == 0)
        {
            unitVector = direction;
        }
        else
        {
            unitVector = direction / (direction.magnitude);
        }
        unitVector *= distance;
        return (new Vector3(origin.x + unitVector.x, origin.y, origin.z + unitVector.y));
    }


    public void setDirection(Vector3 _direction)
    {
        direction = _direction;
    }
}
