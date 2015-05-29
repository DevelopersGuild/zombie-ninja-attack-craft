using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomUnlock : MonoBehaviour
{

     public List<Door> doors = new List<Door>();
     public List<GameObject> enemies = new List<GameObject>();
     public List<GameObject> rewards = new List<GameObject>();

     private bool isCleared;
     private bool isActive;

     // Make sure the doors are open and the enemies arent spawned
     void Start()
     {
          isCleared = false;
          isActive = false;

          foreach (Door door in doors)
          {
               Debug.Log("preopened" + doors.Count);
               door.OpenDoor();
               Debug.Log("opened" + doors.Count);
          }
          foreach (GameObject enemy in enemies)
          {
               enemy.SetActive(false);
               Debug.Log("enemies");
          }
          foreach (GameObject reward in rewards)
          {
               reward.SetActive(false);
          }
     }

     // Check for the player to activate the room
     void OnTriggerEnter2D(Collider2D other)
     {
          if (other.tag == "Player")
          {
               ActivateRoom();
          }
     }

     // Update is called once per frame
     void Update()
     {
          // Check for whether or not all the enemies in the room have died
          if (isActive)
          {
               isCleared = true;
               foreach (GameObject enemy in enemies)
               {
                    if (enemy != null)
                    {
                         isCleared = false;
                    }
               }
               if (isCleared)
               {
                    closeRoom();
               }
          }
     }

     // Close the doors and spawn all the enemies
     void ActivateRoom()
     {
          foreach (Door door in doors)
          {
               door.CloseDoor();
          }
          foreach (GameObject enemy in enemies)
          {
               enemy.SetActive(true);
          }
          isActive = true;
     }

     // Open the doors and spawn the reward items
     void closeRoom()
     {
          foreach (Door door in doors)
          {
               door.OpenDoor();
          }
          foreach (GameObject reward in rewards)
          {
               reward.SetActive(true);
               Debug.Log("rewards!");
          }
          //Destroy(gameObject);
     }
}
