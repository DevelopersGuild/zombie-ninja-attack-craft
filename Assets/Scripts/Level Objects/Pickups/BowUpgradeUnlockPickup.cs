using UnityEngine;
using System.Collections;

public class BowUpgradeUnlockPickup : Pickup
{
     public override void sendPickupMessage()
     {
          GameManager.Notifications.PostNotification(this, "UnlockPowerShot");
     }
}
