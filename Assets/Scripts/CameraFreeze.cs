using UnityEngine;
using System.Collections;

public class CameraFreeze : MonoBehaviour {

     public Transform freezePosition;
     private CameraFollow camera;
	// Use this for initialization
     void Start()
     {
         camera = Camera.main.GetComponent<CameraFollow>();
     }
     // Freeze the position when the player enters the collider
	void OnTriggerEnter2D(Collider2D other)
     {
          if (other.GetComponent<Player>())
          {
               camera.playerPosition = freezePosition;
          }

     }

     public void UnfreezeToPlayer()
     {
          camera.playerPosition = camera.player.transform;
     }
}
