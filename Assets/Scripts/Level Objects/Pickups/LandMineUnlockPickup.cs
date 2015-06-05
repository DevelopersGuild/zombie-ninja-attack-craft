using UnityEngine;
using System.Collections;

public class LandMineUnlockPickup :Pickup
{
     public override void sendPickupMessage()
     {
          GameManager.Notifications.PostNotification(this, "UnlockGrenade");
     }
}
