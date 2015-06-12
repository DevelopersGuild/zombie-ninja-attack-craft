using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;


public class ProjectileTerrain : Projectile
{

     public GameObject area, areaObj;
     public float areaTimeToLive;

     public void Start()
     {
          player = FindObjectOfType<Player>();
          direction = transform.position - player.transform.position;
          direction *= -1;
     }

     public void Update()
     {
          if (TimeToLive <= 0)
          {
               if (areaObj != null)
               {
                    area = Instantiate(areaObj, transform.position, transform.rotation) as GameObject;
                    Destroy(area, areaTimeToLive);
               }
               Destroy(gameObject);
          }
          if (AutoTileMap.Instance.GetAutotileCollisionAtPosition(transform.position) == AutoTileMap.eTileCollisionType.BLOCK)
          {
               if (areaObj != null)
               {
                    area = Instantiate(areaObj, transform.position, transform.rotation) as GameObject;
                    Destroy(area, areaTimeToLive);
               }
               Destroy(transform.gameObject);
          }
          TimeToLive -= Time.deltaTime;
     }


     public void Shoot(float angle, Vector2 velocity, int damage = 1)
     {
          damageAmount = damage;
          originalPosition = transform.position;
          transform.eulerAngles = new Vector3(0, 0, angle);
          currentVelocity = velocity;
          GetComponent<Rigidbody2D>().velocity = currentVelocity * projectileSpeed;
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
               if (areaObj != null)
               {
                    area = Instantiate(areaObj, transform.position, transform.rotation) as GameObject;
                    Destroy(area, areaTimeToLive);
                    player.setStun(stun);

               }
               Destroy(gameObject);
          }

     }

}
