using UnityEngine;
using System.Collections;

public class AmmoPickup : Pickup
{

     private AttackController attackController;


     public override void AddItemToInventory(Collider2D player, int value)
     {
          attackController = player.gameObject.GetComponent<AttackController>();
          attackController.ammo += value;
     }
}
