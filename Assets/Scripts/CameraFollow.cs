using UnityEngine;
using System.Collections;
using DG.Tweening;


public class CameraFollow : MonoBehaviour {

     public Player player;
     private Transform transform;
     private bool isShaking;
     public Transform playerPosition;

	// Use this for initialization
     void Awake()
     {
          isShaking = false;
          DOTween.KillAll();
     }
	void Start () {
          DOTween.Init(true, false);
          player = FindObjectOfType<Player>();
          transform = GetComponent<Transform>();
          playerPosition = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
          if (isShaking)
          {
               Tween cameraShake = transform.DOShakePosition(.05f, new Vector3(.15f,.15f,0), 5).OnComplete(DoneShaking);
          }
          else
          {
               transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, -10);
          }
     }

     public void CameraShake()
     {
          isShaking = true;
     }
     public void DoneShaking()
     {
          isShaking = false;
          transform.DOMove(new Vector3(playerPosition.position.x, playerPosition.position.y, -10), 0.1f);
     }
     public void Test()
     {
          transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, -10);
     }
}
