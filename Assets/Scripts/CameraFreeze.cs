using UnityEngine;
using System.Collections;

public class CameraFreeze : MonoBehaviour {

     public Transform freezePosition;
	// Use this for initialization
	void OnTriggerEnter2D()
     {
          CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
          camera.playerPosition = freezePosition;
     }
}
