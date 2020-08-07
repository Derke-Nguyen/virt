//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;


//Abstract class definition which all other states inherit from
public abstract class Enemy1BaseState
{
    public abstract void EnterState(Enemy1 enemy);

    public abstract void Update(Enemy1 enemy);

    public abstract void OnCollisionEnter(Enemy1 enemy);

    public abstract void FixedStateUpdate(Enemy1 enemy);
}
