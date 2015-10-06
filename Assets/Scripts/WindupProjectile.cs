using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

public class WindupProjectile : Projectile
{


     [HideInInspector]
     public Vector2 maxVelocity;
     private float speedBoost, temp;

     public void Start()
     {
          Destroy(transform.gameObject, TimeToLive);
          homing = shot = false;
          player = FindObjectOfType<Player>();
          speedBoost = 0;
     }

     public void Update()
     {
          
          if (speedBoost >= TimeToLive/100.0) {
               speedBoost = 0;
               if (currentVelocity.magnitude < maxVelocity.magnitude)
               {
                    currentVelocity *= 1.1f;
                    GetComponent<Rigidbody2D>().velocity = currentVelocity;
               }
          }
          
          if (AutoTileMap.Instance.GetAutotileCollisionAtPosition(transform.position) == AutoTileMap.eTileCollisionType.BLOCK)
          {
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
          speedBoost += Time.deltaTime;
          temp += Time.deltaTime;
     }

     public void setStun(float st)
     {
          stun = st;

     }

     public void home(bool x)
     {
          homing = x;
     }



     public void Shoot(float angle, Vector2 velocity, int damage = 1)
     {
          damageAmount = damage;
          originalPosition = transform.position;
          currentAngle = angle;
          transform.eulerAngles = new Vector3(0, 0, currentAngle);
          maxVelocity = velocity * projectileSpeed;
          currentVelocity = maxVelocity * 0.01f;
          GetComponent<Rigidbody2D>().velocity = currentVelocity;
          shot = true;
     }



     public float getStun()
     {
          return stun;
     }


}



