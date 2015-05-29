using UnityEngine;
using System.Collections;

public class DealDamageToEnemy : MonoBehaviour {

    public int damageAmount;

    //For colliders
    public void OnCollisionStay2D(Collision2D other) {
        //Debug.Log("COLLIDER!");

        //Check for enemy collision
        if (other.gameObject.CompareTag("Attackable"))
        {
            //Find components necessary to take damage and knockback
            GameObject enemObject = other.gameObject;
            Health enemyHealth = other.gameObject.GetComponent<Health>();
            if (enemObject.GetComponent<Enemy>() != null)
            {
                Enemy enemy = enemObject.GetComponent<Enemy>();

                //Deal damage and knockback the enemy
                if (!enemy.isInvincible)
                {
                    //Deal damage, knockback, set the invinicility flag
                    enemyHealth.CalculateKnockback(other, transform.position);
                    enemyHealth.TakeDamage(damageAmount);
                    enemy.isInvincible = true;
                }
            }
            else if (enemyHealth.GetComponent<Rigidbody2D>())
            {
                enemyHealth.CalculateKnockback(other, transform.position);
            }
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            Debug.Log("Heyo");
            GameObject enemObject = other.gameObject;
            Boss enemy = enemObject.GetComponent<Boss>();
            Health enemyHealth = enemObject.GetComponent<Health>();
            Debug.Log("Ooh baebii" + other.gameObject.tag);

            //Deal damage and knockback the enemy
            if (!enemy.isInvincible)
            {
                //Deal damage, knockback, set the invinicility flag
                enemyHealth.CalculateKnockback(other, transform.position);
                enemyHealth.TakeDamage(damageAmount);
                enemy.isInvincible = true;
            }
        }

        //Destroy gameobject if its a projectile
        if (GetComponent<Projectile>()) {
            Destroy(gameObject);
         }

    }

    //For triggers
    public void OnTriggerEnter2D(Collider2D other) {
       // Debug.Log("TRIGGER!");
        //Check for enemy collision
        if (other.gameObject.CompareTag("Attackable"))
        {
            //Find components necessary to take damage and knockback
            GameObject enemObject = other.gameObject;
            Health enemyHealth = other.gameObject.GetComponent<Health>();
            Debug.Log("Smack him");
            if (enemObject.GetComponent<Enemy>() != null)
            {
                Debug.Log("getting closer");
                Enemy enemy = enemObject.GetComponent<Enemy>();

                //Deal damage and knockback the enemy
                if (!enemy.isInvincible)
                {
                    Debug.Log("Success");
                    //Deal damage, knockback, set the invinicility flag
                    enemyHealth.CalculateKnockback(other, transform.position);
                    enemyHealth.TakeDamage(damageAmount);
                    enemy.isInvincible = true;
                }
            }
            else if (enemyHealth.GetComponent<Rigidbody2D>())
            {
                enemyHealth.CalculateKnockback(other, transform.position);
            }
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            Debug.Log("Heyo");
            GameObject enemObject = other.gameObject;
            Boss enemy = enemObject.GetComponent<Boss>();
            Health enemyHealth = enemObject.GetComponent<Health>();

            //Deal damage and knockback the enemy
            if (!enemy.isInvincible)
            {
                //Deal damage, knockback, set the invinicility flag
                enemyHealth.CalculateKnockback(other, transform.position);
                enemyHealth.TakeDamage(damageAmount);
                enemy.isInvincible = true;
            }
        }

        //Destroy itself if its a projectile
        if (GetComponent<Projectile>()) {
            Destroy(gameObject);
        }
    }
}
