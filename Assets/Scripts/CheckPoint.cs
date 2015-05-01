using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour 
{
     public NotificationManger Notifications = null;
     private Vector3 respawnLocation;
     public Player player = null;

	// Use this for initialization
	void Start() 
     {
          Notifications.AddListener(this, "OnPlayerDeath");
          Notifications.AddListener(this, "ActivateCheckPoint");
          ActivateCheckPoint();
	}

     public void OnPlayerDeath(Component Sender)
     {
          Respawn();
     }
	

     void ActivateCheckPoint()
     {
          respawnLocation = player.transform.position;
     }

     void Respawn()
     {
          player.transform.position = respawnLocation;
     }
}
