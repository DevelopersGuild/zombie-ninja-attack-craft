using UnityEngine;
using System.Collections;


public class DealDamageToEnemy : MonoBehaviour
{

    public int damageAmount = 1;
    public int NumberOfEnemiesCanHit = 2;
    private Projectile projectile;
    private bool isProjectile = false;
    private bool isPowerShot = false;
    private int enemiesHit = 0;

    //For colliders
    public void OnCollisionStay2D(Collision2D other)
    {
        //Debug.Log("COLLIDER!");

        CheckForProjectile();
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

            enemiesHit++;
        }

        //Destroy gameobject if its a projectile
        ProjectileDestroy(isProjectile);
    }

    //For triggers
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("TRIGGER!");
        CheckForProjectile();
        //Check for enemy collision
        if (other.tag == "Attackable")
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

            enemiesHit++;
        }

        //Destroy itself if its a projectile
        ProjectileDestroy(isProjectile);
    }

    public void CheckForProjectile()
    {
        if (projectile = GetComponent<Projectile>())
        {
            damageAmount = projectile.damageAmount;
            if (damageAmount > 1)
            {
                isPowerShot = true;
            }
            isProjectile = true;
        }
    }

    public void ProjectileDestroy(bool isObjectProjectile)
    {
        if (isPowerShot == true)
        {
            if (isObjectProjectile == true && enemiesHit == NumberOfEnemiesCanHit)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (isObjectProjectile == true)
            {
                Destroy(gameObject);
            }
        }

    }
}
