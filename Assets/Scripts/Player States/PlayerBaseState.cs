//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerController player);

    public abstract void Update(PlayerController player);

    public abstract void OnCollisionEnter(PlayerController player);

    public abstract void FixedStateUpdate(PlayerController player);
}
