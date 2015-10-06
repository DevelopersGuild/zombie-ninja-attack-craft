using UnityEngine;
using System.Collections;

public class AnimationEvent : MonoBehaviour {

     public Player player;

	// Use this for initialization
	void Start () {
          player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

     public void OnTriggerEnter2D(Collider2D other)
     {
          if(other.gameObject.CompareTag("Player"))
          {
               player.arrowKeys.canTakeInput = false;
               player.GetComponent<PlayerMoveController>().Move(0, 0);

          }
     }
     public void OnTriggerStay2D(Collider2D other)
     {

          if (other.gameObject.CompareTag("Player"))
          {
               player.GetComponent<PlayerMoveController>().canMove = false;
               player.GetComponent<PlayerMoveController>().Move(0, 0);
          }
     }
}
