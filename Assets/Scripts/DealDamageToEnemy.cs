using UnityEngine;
using System.Collections;

public class DealDamageToEnemy : MonoBehaviour {

    public int damageAmount;

    //For colliders
    public void OnCollisionStay2D(Collision2D other) {
        //Debug.Log("COLLIDER!");

        //Check for enemy collision
        if (other.gameObject.CompareTag("Attackable")|| other.gameObject.CompareTag("Boss")) {
            //Find components necessary to take damage and knockback
            Health enemyHealth = other.gameObject.GetComponent<Health>();
            Debug.Log("Ooh baebii" + other.gameObject.tag);

            //Deal damage and knockback the enemy
            enemyHealth.CalculateKnockback(other, transform.position);
            enemyHealth.TakeDamage(damageAmount);
        } else

        //Destroy gameobject if its a projectile
        if (GetComponent<Projectile>()) {
            Destroy(gameObject);
         }

    }

    //For triggers
    public void OnTriggerEnter2D(Collider2D other) {
       // Debug.Log("TRIGGER!");
        //Check for enemy collision
        if (other.tag == "Attackable") {
            //Deal damage, knock back what it collided with, and destory itselfz
            Health enemyHealth = other.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(1);
            if (enemyHealth.GetComponent<Rigidbody2D>()) {
                enemyHealth.CalculateKnockback(other, transform.position);
            }
        }

        //Destroy itself if its a projectile
        if (GetComponent<Projectile>()) {
            Destroy(gameObject);
        }
    }
}
