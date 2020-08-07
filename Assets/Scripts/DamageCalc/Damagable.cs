using UnityEngine;


//Superclass of all damagable objects (Player, Target Dummies, etc.)
public interface Damagable
{
    void takeHit(float damage);
}
