using UnityEngine;
using System.Collections;

namespace CreativeSpore
{
	[RequireComponent(typeof(Camera))]
	public class Camera2DController : MonoBehaviour {


		Camera m_camera;

		public float Zoom = 1f;
		public float PixelToUnits = 100f;

		// Use this for initialization
		void Start () 
		{
			m_camera = GetComponent<Camera>();
		}
		
		Vector3 m_vCamRealPos;
		void LateUpdate () 
		{
			//Note: ViewCamera.orthographicSize is not a real zoom based on pixels. This is the formula to calculate the real zoom.
			m_camera.orthographicSize =  (Screen.height)/(2f*Zoom*PixelToUnits);
			Vector3 vOri = m_camera.ScreenPointToRay( Vector3.zero ).origin;

			m_vCamRealPos = m_camera.transform.position;
			Vector3 vPos = m_camera.transform.position;
			float mod = (1f/(Zoom*PixelToUnits));
			vPos.x -= vOri.x % mod;
			vPos.y -= vOri.y % mod;
			m_camera.transform.position = vPos;
		}

		void OnPostRender()
		{
			m_camera.transform.position = m_vCamRealPos;
		}
	}
}
