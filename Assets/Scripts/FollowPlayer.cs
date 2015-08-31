using UnityEngine;
using System.Collections;

public class FollowPlayer: MonoBehaviour {

     private Transform transform;
     public Transform target;

	// Use this for initialization
	void Start () {
          transform = GetComponent<Transform>();
          target = FindObjectOfType<Player>().GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
          transform.position = target.position;
	}
}
