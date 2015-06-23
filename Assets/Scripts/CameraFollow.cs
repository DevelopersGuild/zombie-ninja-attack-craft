using UnityEngine;
using System.Collections;
using DG.Tweening;


public class CameraFollow : MonoBehaviour {

     public Player player;
     public Transform playerPosition;

	// Use this for initialization
	void Start () {
          player = FindObjectOfType<Player>();
          playerPosition = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, -10);

        if (player.gotAttacked == true)
        {
             CameraShake();
        }
     }

     public void CameraShake()
     {
          transform.DOShakePosition(0.40f, 0.15f, 45, 45);
     }
}
