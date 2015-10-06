using UnityEngine;
using System.Collections;

public class BombAmmoPickup : Pickup 
{
     private AttackController attackController;
     public override void AddItemToInventory(Collider2D player, int value)
     {
          attackController = player.gameObject.GetComponent<AttackController>();
          attackController.Grenades += value;
     }

     public override void sendPickupMessage()
     {
          GameManager.Notifications.PostNotification(this, "OnBombOrBatteryPickup");
     }
	
}
