using UnityEngine;
using System.Collections;
using DG.Tweening;


public class CameraFollow : MonoBehaviour {

    public Transform playerPosition;

	// Use this for initialization
	void Start () {
          playerPosition = FindObjectOfType<Player>().transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, -10);
	}

     public void CameraShake()
     {

     }
}
