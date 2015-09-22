using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

public class Projectile : MonoBehaviour
{
     public float projectileSpeed;
     public float angle;
     public float currentAngle;
     public float damageAmount;
     public int pierceAmount;
     public ParticleSystem destroyParticle;

     public float stun;
     [HideInInspector]
     public bool homing, shot;

     public float TimeToLive;

     [HideInInspector]
     public Vector2 PlayerPos, direction;
     public Player player;
     public Vector2 originalPosition;
     public Vector2 targetPosition;
     public Vector2 currentVelocity;

     public void Start()
     {
          Destroy(transform.gameObject, TimeToLive);
          homing = shot = false;
          player = FindObjectOfType<Player>();
          if (GetComponent<DealDamageToShieldBoss>())
               damageAmount = 1;
          else
               damageAmount = GetComponent<DealDamageToPlayer>() ? GetComponent<DealDamageToPlayer>().damageAmount : GetComponent<DealDamageToEnemy>().damageAmount;
     }

     public void Update()
     {
          if (AutoTileMap.Instance.GetAutotileCollisionAtPosition(transform.position) == AutoTileMap.eTileCollisionType.BLOCK)
          {
               if (destroyParticle)
               {
                    Instantiate(destroyParticle, transform.position, Quaternion.identity);
               }
               Destroy(transform.gameObject);
          }
          if (shot)
          {
               if (homing)
               {
                    PlayerPos = player.transform.position;
                    direction = new Vector2(PlayerPos.x - transform.position.x, PlayerPos.y - transform.position.y);
                    GetComponent<Rigidbody2D>().velocity += direction.normalized * 0.2f;
               }
          }
     }

     public void OnDestroy()
     {
          if (destroyParticle)
          {
               Instantiate(destroyParticle, transform.position, Quaternion.identity);
          }
     }

     public void OnTriggerEnter2D(Collider2D other)
     {
          if (other.gameObject.layer == LayerMask.NameToLayer("CollidableObjects"))
          {
               Destroy(gameObject);
          }
     }

     public void setStun(float st)
     {
          stun = st;
     }

     public void home(bool x)
     {
          homing = x;
     }



     public void Shoot(float angle, Vector2 velocity)
     {
          originalPosition = transform.position;
          currentAngle = angle;
          transform.eulerAngles = new Vector3(0, 0, currentAngle);
          currentVelocity = velocity;
          GetComponent<Rigidbody2D>().velocity = currentVelocity * projectileSpeed;
          shot = true;
     }


     public float getStun()
     {
          return stun;
     }


}



