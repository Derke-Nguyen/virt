using UnityEngine;
using UnityEngine.SceneManagement;


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
            if(this.tag == "Player")
            {
                SceneManager.LoadScene("Game Over"); //Transitions to game over when player dies
                return;
            }
            DIE(); //Dies when health reaches 0
        }
    }

    //Marks object as dead and destroys it
    public void DIE()
    {
        dead = true;
        //Debug.Log(this.name + " is dead");
        GameObject.Destroy(gameObject); //Destroys object when dead
    }
}
