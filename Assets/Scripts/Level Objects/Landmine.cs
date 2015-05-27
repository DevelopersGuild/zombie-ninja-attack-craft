using UnityEngine;
using System.Collections;

public class Landmine : MonoBehaviour
{

     public Explosion explosion;
     public SpriteRenderer sprite;

     private bool isActive;
     public float timeToExplode;
     private float currentTime;
     private Collider2D enemy;

     // Use this for initialization
     void Start()
     {
          isActive = false;
          currentTime = 0;
          sprite = GetComponent<SpriteRenderer>();
          sprite.sortingLayerName = "Pickups";
     }

     // Update is called once per frame
     void Update()
     {
          if (isActive)
          {
               currentTime += Time.deltaTime;

               //The sprite blinks once the player has stepped in its range and explodes afterwards
               if (currentTime <= timeToExplode)
               {
                    float remainder = currentTime % .1f;
                    GetComponent<Renderer>().enabled = remainder > .05f;
               }
               else
               {
                    Instantiate(explosion, transform.position, transform.rotation);

                    Destroy(gameObject);
               }
          }
     }

     void OnTriggerEnter2D(Collider2D other)
     {
          //Set the landmine to active and explode if the player has stepped into its trigger

          if (other.gameObject.tag == "Attackable")
          {
               isActive = true;
          }

     }
}
