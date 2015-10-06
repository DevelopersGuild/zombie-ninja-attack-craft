using UnityEngine;
using System.Collections;

public class CameraFreeze : MonoBehaviour
{

     public Transform freezePosition;
     private CameraFollow camera;
     private bool isComplete;
     // Use this for initialization
     void Start()
     {
          isComplete = false;
          camera = Camera.main.GetComponent<CameraFollow>();
     }
     // Freeze the position when the player enters the collider
     void OnTriggerEnter2D(Collider2D other)
     {
          if (other.GetComponent<Player>() && isComplete == false)
          {
               camera.playerPosition = freezePosition;
          }

     }

     public void UnfreezeToPlayer()
     {
          camera.playerPosition = camera.player.transform;
          isComplete = true;
     }
}
