using UnityEngine;
using System.Collections;

public class BombScript : Pickup
{
     public Explosion explosion;
     private AttackController attackController;

     public float timeToExplode;
     private float currentTime;

     // Update is called once per frame
     void Update()
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

     public override void AddItemToInventory(Collider2D player, int value)
     {
          attackController = player.gameObject.GetComponent<AttackController>();
          attackController.Grenades += value;
     }
}
