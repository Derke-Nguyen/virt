using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : LivingEntity
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract bool inTheRed();
}
