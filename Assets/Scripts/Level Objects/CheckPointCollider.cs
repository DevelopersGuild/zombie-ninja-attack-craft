using UnityEngine;
using System.Collections;

public class CheckPointCollider : MonoBehaviour 
{

     public NotificationManager Notifications = null;

     void OnTriggerEnter2D(Collider2D collider)
     {
          if (collider.gameObject.tag == "Player")
          {
               Notifications.PostNotification(this, "ActivateCheckPoint");
               Destroy(gameObject);
          }
     }
}
