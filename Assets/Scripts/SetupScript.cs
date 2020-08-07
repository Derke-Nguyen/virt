using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupScript : MonoBehaviour
{
    GameObject[] Enemies;

    public Transform here;
    public GameObject QuestItem;

    bool generated = false;

    // Start is called before the first frame update
    void Start()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (Enemies.Length == 0 && !generated)
        {
            generated = true;
            Debug.Log("All Enemies Defeated");
            Instantiate(QuestItem, here.position, here.rotation);
        }
    }
}
