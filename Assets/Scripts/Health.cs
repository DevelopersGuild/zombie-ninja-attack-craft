using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
     public int startingHealth = 10;
     public int currentHealth;
     public int CoinValue;
     public bool isDead;

     private bool isQuitting;

     private Animator anim;
     private AudioSource enemyAudio;
     public ParticleSystem particle;
     public ParticleSystem deathParticle;
     private CameraFollow camera;

     public bool canKnock;

     // Use this for initialization
     void Start()
     {
          isDead = false;
          currentHealth = startingHealth;
          canKnock = true;
          camera = FindObjectOfType<CameraFollow>();
     }

     public void cancelKnockback()
     {
          canKnock = false;
     }

     public void replenish(int amt)
     {
          currentHealth += amt;
          if (currentHealth > startingHealth)
               currentHealth = startingHealth;
     }

     public void setHealth(int amt)
     {
          currentHealth = amt;
     }
     public int currentHp()
     {
          return currentHealth;
     }

     public void TakeDamage(int amount)
     {
          GameManager.Notifications.PostNotification(this, "OnHit");
          currentHealth -= amount;

          // Instantiate a particle effect if it has one
          if (particle != null && currentHealth > 0)
          {
               Instantiate(particle, transform.position, transform.rotation);
          }
          else if (deathParticle != null && currentHealth <= 0)
          {
               Instantiate(deathParticle, transform.position, transform.rotation);
          }

          // Shake the camera if its not a barrel
          if(!gameObject.CompareTag("Barrel"))
          {
               camera.CameraShake();
          }   

          if (currentHealth <= 0)
          {
               Death();
          }
     }

     //For triggers
     public void CalculateKnockback(Collider2D other, Vector2 currentPosition)
     {
          //Calculate point of collision and knockback accordingly
          Vector3 contactPoint = other.transform.position;
          Vector3 center = currentPosition;
          EnemyMoveController enemyMoveController = other.gameObject.GetComponent<EnemyMoveController>();
          PlayerMoveController playerMoveController = other.gameObject.GetComponent<PlayerMoveController>();

          if (canKnock)
          {
               if (enemyMoveController != null)
               {
                    Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
                    enemyMoveController.Knockback(pushDirection.normalized);
               }
               else
               {
                    Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
                    playerMoveController.Knockback(pushDirection.normalized);
               }
          }

     }

     //For colliders
     public void CalculateKnockback(Collision2D other, Vector2 currentPosition)
     {
          //Calculate point of collision and knockback accordingly
          Vector3 contactPoint = other.transform.position;
          Vector3 center = currentPosition;
          EnemyMoveController enemyMoveController = other.gameObject.GetComponent<EnemyMoveController>();
          PlayerMoveController playerMoveController = other.gameObject.GetComponent<PlayerMoveController>();

          if (enemyMoveController != null)
          {
               Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
               enemyMoveController.Knockback(pushDirection.normalized);
          }
          else if (playerMoveController != null)
          {
               Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
               playerMoveController.Knockback(pushDirection.normalized);
          }

     }

     public void Death()
     {
          isDead = true;
          if (gameObject.tag == "Player")
          {
               GameManager.Notifications.PostNotification(this, "OnPlayerDeath");
               this.setHealth(startingHealth);
          }
          else if (gameObject.GetComponent<Enemy>())
          {
               Enemy enem = gameObject.GetComponent<Enemy>();
               enem.onDeath();
          }
          else if (gameObject.GetComponent<Boss>())
          {
               Boss enem = gameObject.GetComponent<Boss>();
               enem.onDeath();
          }
          isDead = true;

          DropLoot dropLoot;
          if (dropLoot = GetComponent<DropLoot>())
          {
               //Dont drop loot if its a enemy spawning barrel
               if (!GetComponent<BarrelSpawn>())
               {
                    dropLoot.DropItem();
               }
          }


          Destroy(gameObject);


     }

     void OnApplicationQuit()
     {
          isQuitting = true;
     }

     //Drop loot on death
     public void OnDestroy()
     {
          if (!isQuitting)
          {

          }
     }
}

