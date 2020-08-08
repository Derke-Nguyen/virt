using UnityEngine;
using UnityEngine.UI;


//Superclass of all Living Entities (Player, Target Dummies, etc.)
public class LivingEntity : MonoBehaviour, Damagable
{
    public float startingHealth;
    protected float health;
    protected bool dead;

    public virtual void Start()
    {
        health = startingHealth;
    }

    public virtual void takeHit(float damage)
    {
        health -= damage;
        //Debug.Log("Hit Taken: " + health + " health left, " + damage + " damage taken.");
        if(health <= 0 && !dead)
        {
            DIE(); //Dies when health reaches 0
        }
    }


    //Note: Currently causes error when player dies since camera position is based on player
    //Possible solution: transition to Game Over when player dies
    public void DIE()
    {
        dead = true;
        //Debug.Log(this.name + " is dead");
        GameObject.Destroy(gameObject); //Destroys object when dead
    }
}
