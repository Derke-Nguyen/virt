using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform m_PlayerPosition;

    [SerializeField]
    private float m_DistanceFromTarget = 5f;

    [SerializeField]
    private float m_Yaw = 20;
    [SerializeField]
    private float m_Pitch = 90;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector3 targetRotation = new Vector3(m_Pitch, m_Yaw);
        transform.eulerAngles = targetRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_PlayerPosition.position - transform.forward * m_DistanceFromTarget;
    }
}
