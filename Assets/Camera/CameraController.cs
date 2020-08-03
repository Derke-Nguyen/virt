using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
 
    private Transform m_PlayerPosition;

    [SerializeField]
    private float m_DistanceFromTarget = 5f;

    [SerializeField]
    private float m_Yaw = 45;
    [SerializeField]
    private float m_Pitch = 45;

    // Start is called before the first frame update
    void Start()
    {
        //find the game object with tag "Player", should only be one in the entire scene
        m_PlayerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //sets rotation based on given pitch and yaw
        Vector3 targetRotation = new Vector3(m_Pitch, m_Yaw);
        transform.eulerAngles = targetRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //move camera along with player position
        transform.position = m_PlayerPosition.position - transform.forward * m_DistanceFromTarget;
    }
}
