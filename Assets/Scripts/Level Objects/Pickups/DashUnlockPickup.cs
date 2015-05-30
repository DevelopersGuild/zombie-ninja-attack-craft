using UnityEngine;
using System.Collections;

public class DashUnlockPickup : Pickup
{
     public override void sendPickupMessage()
     {
          GameManager.Notifications.PostNotification(this, "UnlockDash");
     }
}
