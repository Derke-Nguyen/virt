//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;


//Abstract class definition which all other states inherit from
public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerController player);

    public abstract void Update(PlayerController player);

    public abstract void OnCollisionEnter(PlayerController player);

    public abstract void FixedStateUpdate(PlayerController player);
}
