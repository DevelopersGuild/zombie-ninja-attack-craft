using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;


public class Homer : MonoBehaviour
{

     public float projectileSpeed;
     public float angle;
     public float range;
     public Player player;

     public float TimeToLive;

     public Vector2 originalPosition;
     public Vector2 targetPosition;
     public Vector2 currentVelocity;
     private Vector2 direction;

     public void Start()
     {
          Destroy(transform.gameObject, TimeToLive);
          player = FindObjectOfType<Player>();
          direction = transform.position - player.transform.position;
          direction *= -1;
     }

     public void Update()
     {
          Shoot(direction, projectileSpeed);
          if (AutoTileMap.Instance.GetAutotileCollisionAtPosition(transform.position) == AutoTileMap.eTileCollisionType.BLOCK)
          {
               Destroy(transform.gameObject);
          }
     }

     public void Shoot(Vector2 direction, float projSpd)
     {
          currentVelocity = direction * projSpd;
          GetComponent<Rigidbody2D>().velocity = currentVelocity;
     }




}
