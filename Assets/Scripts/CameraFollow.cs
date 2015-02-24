using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform playerPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, -10);
	}
}
