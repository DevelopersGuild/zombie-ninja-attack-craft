using UnityEngine;
using System.Collections;

public class HealthPickup : Pickup
{
     private Health playerHealth;
     public override void AddItemToInventory(Collider2D player, int value)
     {
          playerHealth = player.GetComponent<Health>();
          playerHealth.replenish(value);
     }

     public override void sendPickupMessage()
     {
          GameManager.Notifications.PostNotification(this, "OnHealthOrPickup");
     }
}
