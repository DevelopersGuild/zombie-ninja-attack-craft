using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

public class SnakeBall : MonoBehaviour
{

     public Vector2 initialPos;
     public float pingpong;

     public void Start()
     {
         
     }

     public void Update()
     {
          pingpong = Mathf.PingPong(Time.time / 2.2f, 0.7f);
          transform.position = new Vector3(initialPos.x + pingpong, initialPos.y, 0);
     }

     public void dead()
     {
          Destroy(gameObject);
     }



}