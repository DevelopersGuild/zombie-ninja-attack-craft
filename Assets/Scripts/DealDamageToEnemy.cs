using UnityEngine;
using System.Collections;

public class DealDamageToEnemy : MonoBehaviour
{

     public int damageAmount = 1;
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
          if (other.gameObject.tag == "Attackable")
          {
               //Find components necessary to take damage and knockback
               Health enemyHealth = other.gameObject.GetComponent<Health>();

               //Deal damage and knockback the enemy
               enemyHealth.CalculateKnockback(other, transform.position);
               enemyHealth.TakeDamage(damageAmount);
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
               //Deal damage, knock back what it collided with, and destory itselfz
               Health enemyHealth = other.gameObject.GetComponent<Health>();
               enemyHealth.TakeDamage(damageAmount);
               if (enemyHealth.GetComponent<Rigidbody2D>())
               {
                    enemyHealth.CalculateKnockback(other, transform.position);
               }
               enemiesHit++;
          }

          //Destroy itself if its a projectile
          ProjectileDestroy(isProjectile);
     }

     public void CheckForProjectile()
     {
          if(projectile = GetComponent<Projectile>())
          {
               damageAmount = projectile.damageAmount;
               if(damageAmount > 1)
               {
                    isPowerShot = true;
               }
               isProjectile = true;
          }
     }

     public void ProjectileDestroy(bool isObjectProjectile)
     {
          if(isPowerShot == true)
          {
               if (isObjectProjectile == true && enemiesHit == 2)
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
