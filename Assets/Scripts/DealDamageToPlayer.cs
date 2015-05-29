using UnityEngine;
using System.Collections;

public class DealDamageToPlayer : MonoBehaviour
{

     public int damageAmount;

     public void OnCollisionStay2D(Collision2D other)
     {
          //Check for player collision
          if (other.gameObject.tag == "Player")
          {
               //Find components necessary to take damage and knockback
               GameObject playerObject = other.gameObject;
               Player player = playerObject.GetComponent<Player>();
               Health playerHealth = playerObject.GetComponent<Health>();

               //Take damage if the player isnt already currently invincible
               if (!player.isInvincible)
               {
                    //Deal damage, knockback, set the invinicility flag
                    playerHealth.CalculateKnockback(other, transform.position);
                    playerHealth.TakeDamage(damageAmount);
                    player.isInvincible = true;
               }

               //Destroy gameobject if its a projectile
               if (GetComponent<Projectile>())
               {
                    Destroy(gameObject);
               }
          }
     }
     public void OnTriggerEnter2D(Collider2D other)
     {
          //Check for player collision
          if (other.gameObject.tag == "Player")
          {
               //Find components necessary to take damage and knockback
               GameObject playerObject = other.gameObject;
               Player player = playerObject.GetComponent<Player>();
               Health playerHealth = playerObject.GetComponent<Health>();

               //Take damage if the player isnt already currently invincible
               if (!player.isInvincible)
               {
                    //Deal damage, knockback, set the invinicility flag
                    playerHealth.CalculateKnockback(other, transform.position);
                    playerHealth.TakeDamage(damageAmount);
                    player.isInvincible = true;
               }

               //Destroy gameobject if its a projectile
               if (GetComponent<Projectile>())
               {
                    Destroy(gameObject);
               }
          }

     }
}
