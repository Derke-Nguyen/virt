using UnityEngine;
using UnityEngine.UI;

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
        Debug.Log("Hit Taken: " + health + " health left, " + damage + " damage taken.");
        health -= damage;
        if(health <= 0 && !dead)
        {
            DIE();
        }
    }

    public void DIE()
    {
        dead = true;
        Debug.Log(this.name + " is dead");
        GameObject.Destroy(gameObject);
    }
}
