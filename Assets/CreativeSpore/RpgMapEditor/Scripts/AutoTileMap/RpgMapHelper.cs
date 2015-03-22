using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    public class RpgMapHelper
    {
        public static int GetTileIdxByPosition(Vector3 vPos)
        {
            vPos -= AutoTileMap.Instance.transform.position;

            // transform to pixel coords
            vPos.y = -vPos.y;

            vPos *= AutoTileset.PixelToUnits;
            if (vPos.x >= 0 && vPos.y >= 0)
            {
                int tile_x = (int)vPos.x / AutoTileset.TileWidth;
                int tile_y = (int)vPos.y / AutoTileset.TileWidth;
                return tile_y * AutoTileMap.Instance.MapTileWidth + tile_x;
            }
            else
            {
                return -1;
            }
        }

        public static AutoTileMap.AutoTile GetAutoTileByPosition( Vector3 vPos, int iLayer )
        {
            int idx = GetTileIdxByPosition( vPos );
            if (idx >= 0)
            {
                int tileX = idx % AutoTileMap.Instance.MapTileWidth;
                int tileY = idx / AutoTileMap.Instance.MapTileWidth;
                return AutoTileMap.Instance.GetAutoTile(tileX, tileY, iLayer);
            }
            return new AutoTileMap.AutoTile() { Type = -1 };
        }

        public static Vector3 GetTileCenterPosition( int tile_x, int tile_y )
        {
            Vector3 vPos = new Vector3((tile_x + 0.5f) * AutoTileset.TileWorldWidth, -(tile_y + 0.5f) * AutoTileset.TileWorldHeight);
            return vPos;
        }

        public static void DebugDrawRect( Vector3 pos, Rect rect, Color color )
		{
			rect.position += new Vector2(pos.x, pos.y);
			Debug.DrawLine( new Vector3(rect.x, rect.y, pos.z ), new Vector3(rect.x+rect.width, rect.y, pos.z ), color );
            Debug.DrawLine(new Vector3(rect.x, rect.y, pos.z), new Vector3(rect.x, rect.y + rect.height, pos.z), color);
            Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y, pos.z), new Vector3(rect.x + rect.width, rect.y + rect.height, pos.z), color);
            Debug.DrawLine(new Vector3(rect.x, rect.y + rect.height, pos.z), new Vector3(rect.x + rect.width, rect.y + rect.height, pos.z), color);
		}

        public static void DebugDrawRect(Vector3 pos, Rect rect, Color color, float duration)
        {
            rect.position += new Vector2(pos.x, pos.y);
            Debug.DrawLine(new Vector3(rect.x, rect.y, pos.z), new Vector3(rect.x + rect.width, rect.y, pos.z), color, duration);
            Debug.DrawLine(new Vector3(rect.x, rect.y, pos.z), new Vector3(rect.x, rect.y + rect.height, pos.z), color, duration);
            Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y, pos.z), new Vector3(rect.x + rect.width, rect.y + rect.height, pos.z), color, duration);
            Debug.DrawLine(new Vector3(rect.x, rect.y + rect.height, pos.z), new Vector3(rect.x + rect.width, rect.y + rect.height, pos.z), color, duration);
        }
    }
}
