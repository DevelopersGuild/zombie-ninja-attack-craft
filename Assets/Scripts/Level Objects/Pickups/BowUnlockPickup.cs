using UnityEngine;
using System.Collections;

public class BowUnlockPickup : Pickup
{
     public override void sendPickupMessage()
     {
          GameManager.Notifications.PostNotification(this, "UnlockBow");
     }
}
