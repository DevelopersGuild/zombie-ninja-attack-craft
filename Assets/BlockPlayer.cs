using UnityEngine;
using System.Collections;


public class BlockPlayer : MonoBehaviour
{

     public void OnCollisionStay2D(Collision2D other)
     {
         

     }

     
     public void OnCollisionEnter2D(Collision2D other)
     {


          if (other.gameObject.CompareTag("Player"))
          {
          }
     }

     //For triggers
     public void OnTriggerEnter2D(Collider2D other)
     {


          if (other.gameObject.CompareTag("Player"))
          {
          }
     }

    
}
