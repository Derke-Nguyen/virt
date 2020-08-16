using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class Blade : MonoBehaviour
{
    float damage = 10; // Damage on hit

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Damage calculation for Ripple's Blade
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HERE?");
        if (other.gameObject.tag == "Player") //Enemy damages player when in contact
        {
            Damagable damagableObject = other.GetComponent<Damagable>();
            if (damagableObject != null)
            {
                //Debug.Log("Dealt Damage: " + damage);
                damagableObject.takeHit(damage);
            }
        }
    }
}
