using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

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
            float f0 = Mathf.Abs( AutoTileMap.Instance.OverlayLayerZ - AutoTileMap.Instance.GroundOverlayLayerZ);
            float f1 = Mathf.Abs(AutoTileMap.Instance.transform.position.y - transform.position.y) / (AutoTileMap.Instance.MapTileHeight * AutoTileMap.Instance.Tileset.TileWorldHeight);
            vPos.z = AutoTileMap.Instance.GroundOverlayLayerZ - f0 * f1;
			m_spriteRender.transform.position = vPos;
		}
	}
}