using UnityEngine;
using System.Collections;

public class DealDamageToEnemy : MonoBehaviour
{

     public int damageAmount = 1;
     private Projectile projectile;
     private bool isProjectile = false;

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
          }

          //Destroy itself if its a projectile
          ProjectileDestroy(isProjectile);
     }

     public void CheckForProjectile()
     {
          if(projectile = GetComponent<Projectile>())
          {
               damageAmount = projectile.damageAmount;
               isProjectile = true;
          }
     }

     public void ProjectileDestroy(bool isObjectProjectile)
     {
          if(isObjectProjectile == true)
          {
               Destroy(gameObject);
          }
     }
}
