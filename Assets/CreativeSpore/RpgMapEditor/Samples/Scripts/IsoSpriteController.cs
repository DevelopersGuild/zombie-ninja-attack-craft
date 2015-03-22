using UnityEngine;
using System.Collections;

namespace CreativeSpore
{
	public class IsoSpriteController : MonoBehaviour {

		public SpriteRenderer m_spriteRender;

		// Use this for initialization
		void Start () {
			if (m_spriteRender == null)
			{
				m_spriteRender = GetComponent<SpriteRenderer>();
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			Vector3 vPos = m_spriteRender.transform.position;
			vPos.z = ( Mathf.Floor(transform.position.y*100) * 0.0001f); //100 = pixel size
			m_spriteRender.transform.position = vPos;
		}
	}
}