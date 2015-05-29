using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour
{
     public void OnTriggerEnter2D(Collider2D other)
     {
          if (other.gameObject.tag == "Player")
          {
               GameManager.Notifications.PostNotification(this, "EndOfLevelReached");
          }
     }

}
