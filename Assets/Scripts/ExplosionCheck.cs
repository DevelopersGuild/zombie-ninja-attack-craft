using UnityEngine;
using System.Collections;

public class ExplosionCheck : MonoBehaviour
{
     public GameObject explosion;
     public bool canExplode;

     public void OnCollisionStay2D(Collision2D other)
     {
          //Check for player collision
          if (other.gameObject.tag == "snakeBall")
          {
               if (canExplode)
               {
                    GameObject explode = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
                    Destroy(other.gameObject);
                    Destroy(gameObject);
               }
          }
     }
     public void OnTriggerEnter2D(Collider2D other)
     {
          //Check for player collision
          if (other.gameObject.tag == "snakeBall")
          {
               if (canExplode)
               {
                    GameObject explode = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
                    Destroy(other.transform.root.gameObject);
                  //  Destroy(other.gameObject);
                    Destroy(transform.root.gameObject);
                   // Destroy(gameObject);
               }
          }

     }
     
     public void explode()
     {
          if (canExplode)
          {
               GameObject explode = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
          }
     }
}
