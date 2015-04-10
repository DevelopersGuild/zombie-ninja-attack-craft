﻿using UnityEngine;
using System.Collections;

namespace CreativeSpore
{
	public class FollowObjectBehaviour : MonoBehaviour {

		public float DampTime = 0.15f;
		public Transform Target;

		private Vector3 velocity = Vector3.zero;
		
		// Update is called once per frame
		void Update () 
		{
			if (Target)
			{
				Vector3 point = GetComponent<Camera>().WorldToViewportPoint(Target.position);
				Vector3 delta = Target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
				Vector3 destination = transform.position + delta;
				transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
			}		
		}
	}
}
