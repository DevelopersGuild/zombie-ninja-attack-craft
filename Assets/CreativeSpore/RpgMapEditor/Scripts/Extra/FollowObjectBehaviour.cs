using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace CreativeSpore
{
	public class FollowObjectBehaviour : MonoBehaviour {

		public float DampTime = 0.15f;
        private float test;
		public Transform Target;
        private Player player;

		private Vector3 velocity = Vector3.zero;

        void Start() {
            Target = FindObjectOfType<Player>().transform;
            player = Target.GetComponent<Player>();
        }
		
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

            //test += Time.deltaTime;
            //if (test > 3) {
            //    transform.DOShakePosition(0.40f, 0.15f, 45, 45);
            //    test = 0;
            //}
            //Debug.Log(test);   

            if (player.gotAttacked == true) {

                Target.GetComponent<Player>().gotAttacked = false;
            }
		}

        public void CameraShake() {
            transform.DOShakePosition(0.40f, 0.15f, 45, 45);
        }
	}
}
