using UnityEngine;
using System.Collections;
using DG.Tweening;


public class CameraFollow : MonoBehaviour
{

     public Player player;
     private Transform transform;
     private bool isShaking;

     public Transform playerPosition;
     public float DampTime = 0.15f;

     private Vector3 velocity = Vector3.zero;
     private Camera m_camera;

     // Use this for initialization
     void Awake()
     {
          isShaking = false;
          DOTween.KillAll();
     }
     void Start()
     {
          DOTween.Init(true, false);
          player = FindObjectOfType<Player>();
          transform = GetComponent<Transform>();
          playerPosition = player.transform;
          m_camera = GetComponent<Camera>();
     }

     // Update is called once per frame
     void Update()
     {
          if (isShaking)
          {
               Tween cameraShake = transform.DOShakePosition(.05f, new Vector3(.15f, .15f, 0), 5).OnComplete(DoneShaking);
          }
          else
          {
               if (playerPosition)
               {
                    Vector3 point = m_camera.WorldToViewportPoint(playerPosition.position);
                    Vector3 delta = playerPosition.position - m_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                    Vector3 destination = transform.position + delta;
                    transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
               }
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
}
