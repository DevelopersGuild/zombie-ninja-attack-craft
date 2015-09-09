using UnityEngine;
using System.Collections;


public class DealDamageToEnemy : MonoBehaviour
{

     public float damageAmount;
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

          if (other.gameObject.CompareTag("Attackable") || other.gameObject.CompareTag("Barrel"))
          {
               Debug.Log("DIE!");
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
               else
               {
                    enemyHealth.TakeDamage(damageAmount);
               }
          }
          else if (other.gameObject.CompareTag("Boss"))
          {
               GameObject enemObject = other.gameObject;
               Boss enemy = enemObject.GetComponent<Boss>();
               Health enemyHealth = enemObject.GetComponent<Health>();

               //Deal damage and knockback the enemy
               if (other.gameObject.GetComponent<ShieldBoss>())
               {
                    if (CompareTag("AnonArrow"))
                    {
                         // enemyHealth.CalculateKnockback(other, transform.position);
                         enemyHealth.TakeDamage(damageAmount);
                         enemy.isInvincible = true;
                    }
               }
               else if (!enemy.isInvincible)
               {
                    //Deal damage, knockback, set the invinicility flag
                    // enemyHealth.CalculateKnockback(other, transform.position);
                    enemyHealth.TakeDamage(damageAmount);
                    enemy.isInvincible = true;
                    enemy.setBlink(0.5f);
               }

               enemiesHit++;
          }


          //Destroy itself if its a projectile
          ProjectileDestroy(isProjectile);

     }

     //For triggers
     public void OnCollisionEnter2D(Collision2D other)
     {
          //Debug.Log("TRIGGER!");
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
               else
               {
                    enemyHealth.TakeDamage(damageAmount);
               }
          }
          else if (other.gameObject.CompareTag("Boss"))
          {
               GameObject enemObject = other.gameObject;
               Boss enemy = enemObject.GetComponent<Boss>();
               Health enemyHealth = enemObject.GetComponent<Health>();

               //Deal damage and knockback the enemy
               if (other.gameObject.GetComponent<ShieldBoss>())
               {
                    if (CompareTag("AnonArrow"))
                    {
                         //enemyHealth.CalculateKnockback(other, transform.position);
                         enemyHealth.TakeDamage(damageAmount);
                         enemy.isInvincible = true;
                    }
               }
               else if (!enemy.isInvincible)
               {
                    //Deal damage, knockback, set the invinicility flag
                    // enemyHealth.CalculateKnockback(other, transform.position);
                    enemyHealth.TakeDamage(damageAmount);
                    enemy.isInvincible = true;
                    enemy.setBlink(0.5f);
               }

               enemiesHit++;
          }
          else if (other.gameObject.CompareTag("Barrel"))
          {
               Health enemyHealth = other.gameObject.GetComponent<Health>();
               Debug.Log(damageAmount);
               enemyHealth.TakeDamage(damageAmount);
          }
          //Destroy itself if its a projectile
          ProjectileDestroy(isProjectile);
     }

     //For triggers
     public void OnTriggerEnter2D(Collider2D other)
     {
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
               else
               {
                    enemyHealth.TakeDamage(damageAmount);
               }
          }
          else if (other.gameObject.CompareTag("Boss"))
          {
               GameObject enemObject = other.gameObject;
               Boss enemy = enemObject.GetComponent<Boss>();
               Health enemyHealth = enemObject.GetComponent<Health>();

               //Deal damage and knockback the enemy
               if (other.gameObject.GetComponent<ShieldBoss>())
               {
                    if (CompareTag("AnonArrow"))
                    {
                         //enemyHealth.CalculateKnockback(other, transform.position);
                         enemyHealth.TakeDamage(damageAmount);
                         enemy.isInvincible = true;
                    }
               }
               else if (!enemy.isInvincible)
               {
                    //Deal damage, knockback, set the invinicility flag
                    //enemyHealth.CalculateKnockback(other, transform.position);
                    enemyHealth.TakeDamage(damageAmount);
                    enemy.isInvincible = true;
                    enemy.setBlink(0.5f);

               }

               enemiesHit++;
          }
          else if(other.gameObject.CompareTag("Barrel"))
          {
               Health enemyHealth = other.gameObject.GetComponent<Health>();
               enemyHealth.TakeDamage(damageAmount);
          }

          //Destroy itself if its a projectile
          if(isProjectile)
          {
               ProjectileDestroy(isProjectile);
          }

     }

     public void CheckForProjectile()
     {
          if (projectile = GetComponent<Projectile>())
          {
               isProjectile = true;
          }
     }

     public void ProjectileDestroy(bool isObjectProjectile)
     {
          if (projectile.pierceAmount > 1)
          {
               projectile.pierceAmount--;
               projectile.Shoot(projectile.currentAngle, projectile.GetComponent<Rigidbody2D>().velocity / projectile.projectileSpeed);
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
