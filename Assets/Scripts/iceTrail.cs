using UnityEngine;
using System.Collections;

public class iceTrail : MonoBehaviour
{

     public int newSpeed, slowTime;

     public void OnCollisionStay2D(Collision2D other)
     {
          //Check for player collision
          if (other.gameObject.tag == "Player")
          {
               //Find components necessary to take damage and knockback
               GameObject playerObject = other.gameObject;
               Player player = playerObject.GetComponent<Player>();
               player.setSlow(newSpeed, slowTime);
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
               player.setSlow(newSpeed, slowTime);
          }
     }
}
