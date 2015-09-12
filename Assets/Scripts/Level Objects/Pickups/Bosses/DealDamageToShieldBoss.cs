using UnityEngine;
using System.Collections;

public class DealDamageToShieldBoss : MonoBehaviour
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



          }
          else if (other.gameObject.CompareTag("Boss"))
          {
               GameObject enemObject = other.gameObject;
               Boss enemy = enemObject.GetComponent<Boss>();
               Health enemyHealth = enemObject.GetComponent<Health>();

               if (!enemy.isInvincible)
               {

                    enemyHealth.TakeDamage(damageAmount);
                    enemy.isInvincible = true;
                    enemy.setBlink(0.5f);
               }

          }
          if (GetComponent<Projectile>())
          {
               Destroy(gameObject);
          }
     }

     public void OnTriggerEnter2D(Collider2D other)
     {
          Debug.Log("Pow");
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
                    if (GetComponent<Projectile>())
                    {
                         player.setStun(GetComponent<Projectile>().stun);
                    }
                    //Deal damage, knockback, set the invinicility flag
                    playerHealth.CalculateKnockback(other, transform.position);
                    playerHealth.TakeDamage(damageAmount);
                    player.isInvincible = true;

               }
          }
          else if (other.gameObject.CompareTag("Boss"))
          {
               GameObject enemObject = other.gameObject;
               Boss enemy = enemObject.GetComponent<Boss>();
               Health enemyHealth = enemObject.GetComponent<Health>();

               if (!enemy.isInvincible)
               {

                    enemyHealth.TakeDamage(damageAmount);
                    enemy.isInvincible = true;
                    enemy.setBlink(0.5f);
               }

          }


          if (GetComponent<Projectile>())
          {
               Destroy(gameObject);

          }
     }

     public void OnCollisionEnter2D(Collision2D other)
     {
          Debug.Log("Pow");
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
                    if (GetComponent<Projectile>())
                    {
                         player.setStun(GetComponent<Projectile>().stun);
                    }
                    //Deal damage, knockback, set the invinicility flag
                    playerHealth.CalculateKnockback(other, transform.position);
                    playerHealth.TakeDamage(damageAmount);
                    player.isInvincible = true;

               }
          }
          else if (other.gameObject.CompareTag("Boss"))
          {
               GameObject enemObject = other.gameObject;
               Boss enemy = enemObject.GetComponent<Boss>();
               Health enemyHealth = enemObject.GetComponent<Health>();

               if (!enemy.isInvincible)
               {

                    enemyHealth.TakeDamage(damageAmount);
                    enemy.isInvincible = true;
                    enemy.setBlink(0.5f);
               }

          }


          if (GetComponent<Projectile>())
          {
               Destroy(gameObject);

          }
     }

     public void OnTriggerStay2D(Collider2D other)
     {
          Debug.Log("Pow");
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
                    if (GetComponent<Projectile>())
                    {
                         player.setStun(GetComponent<Projectile>().stun);
                    }
                    //Deal damage, knockback, set the invinicility flag
                    playerHealth.CalculateKnockback(other, transform.position);
                    playerHealth.TakeDamage(damageAmount);
                    player.isInvincible = true;

               }
          }
          else if (other.gameObject.CompareTag("Boss"))
          {
               GameObject enemObject = other.gameObject;
               Boss enemy = enemObject.GetComponent<Boss>();
               Health enemyHealth = enemObject.GetComponent<Health>();

               if (!enemy.isInvincible)
               {

                    enemyHealth.TakeDamage(damageAmount);
                    enemy.isInvincible = true;
                    enemy.setBlink(0.5f);
               }

          }


          if (GetComponent<Projectile>())
          {
               Destroy(gameObject);

          }

     }

}
