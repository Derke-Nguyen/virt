using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform weaponHold;
    public Weapon startingWeapon;
    Weapon currentWeapon;
    void Start()
    {
        currentWeapon = Instantiate(startingWeapon, weaponHold.position, weaponHold.rotation);
        currentWeapon.transform.parent = weaponHold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
