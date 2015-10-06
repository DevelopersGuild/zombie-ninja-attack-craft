using UnityEngine;
using System.Collections;

public class DoorKey : MonoBehaviour
{

     public Door door;

     // Open the door and destory the key object
     void OnTriggerEnter2D(Collider2D other)
     {

          if (other.tag == "Player")
          {
               door.OpenDoor();
               GameManager.Notifications.PostNotification(this, "OnKeyPickup");
               Destroy(gameObject);
          }
     }
}
