using UnityEngine;
using System.Collections;

public class DetectEnemy : MonoBehaviour {

     public EnemyAttack parentCollider;
     private BoxCollider2D collider;

     public void Start()
     {
          collider = GetComponent<BoxCollider2D>();
     }

     void OnTriggerEnter2D(Collider2D other)
     {
          if(other.gameObject.CompareTag("Player")){
               parentCollider.inRange(collider);
          }
     }

     void OnTriggerStay2D(Collider2D other)
     {
          if(other.gameObject.CompareTag("Player")){
               parentCollider.inRange(collider);
          }

     }
}
