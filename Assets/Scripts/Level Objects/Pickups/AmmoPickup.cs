using UnityEngine;
using System.Collections;

public class AmmoPickup : Pickup
{

     public int ammoValue;
     private bool grabbed;
     private AttackController attackController;


     // Use this for initialization
     void Start()
     {
          ammoValue = Random.Range(1, 4);
     }

     void OnTriggerEnter2D(Collider2D other)
     {
          if (other.gameObject.tag == "Player")
          {
               grabbed = true;
               attackController = other.gameObject.GetComponent<AttackController>();
               attackController.Ammo += ammoValue;
               Destroy(gameObject);
          }
     }

     public override void AddItemToInventory(Collider2D player, int value)
     {
          attackController = player.gameObject.GetComponent<AttackController>();
          attackController.Ammo += value;
     }

     public override void sendPickupMessage()
     {
          GameManager.Notifications.PostNotification(this, "OnHealthOrBatteryPickup");
     }
}
