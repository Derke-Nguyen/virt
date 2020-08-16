//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;


//Abstract class definition which all other states inherit from
public abstract class RippleBaseState
{
    public abstract void EnterState(Ripple ripple);

    public abstract void Update(Ripple ripple);

    public abstract void OnCollisionEnter(Ripple ripple);

    public abstract void FixedStateUpdate(Ripple ripple);
}
