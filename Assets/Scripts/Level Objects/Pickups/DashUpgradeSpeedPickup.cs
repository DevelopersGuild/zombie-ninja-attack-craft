using UnityEngine;
using System.Collections;

public class DashUpgradeSpeedPickup : Pickup
{
     public override void sendPickupMessage()
     {
          GameManager.Notifications.PostNotification(this, "UpgradeDashSpeed");
     }
}
