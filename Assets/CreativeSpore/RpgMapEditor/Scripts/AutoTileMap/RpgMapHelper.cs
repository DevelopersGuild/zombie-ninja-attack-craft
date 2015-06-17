using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    /// <summary>
    /// Helper static class with useful methods
    /// </summary>
    public class RpgMapHelper
    {
        /// <summary>
        /// Return the index of the tile containing the world position vPos
        /// </summary>
        /// <param name="vPos"></param>
        /// <returns></returns>
        public static int GetTileIdxByPosition(Vector3 vPos)
        {
            vPos -= AutoTileMap.Instance.transform.position;

            // transform to pixel coords
            vPos.y = -vPos.y;

            vPos *= AutoTileset.PixelToUnits;
            if (vPos.x >= 0 && vPos.y >= 0)
            {
                int tile_x = (int)vPos.x / AutoTileMap.Instance.Tileset.TileWidth;
                int tile_y = (int)vPos.y / AutoTileMap.Instance.Tileset.TileWidth;
                return tile_y * AutoTileMap.Instance.MapTileWidth + tile_x;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Return the AutoTile of the tile containing the world position vPos and located in the layer iLayer ( 0 - ground, 1 - ground overlay, 2 - overlay )
        /// </summary>
        /// <param name="vPos"></param>
        /// <param name="iLayer"></param>
        /// <returns></returns>
        public static AutoTileMap.AutoTile GetAutoTileByPosition( Vector3 vPos, int iLayer )
        {
            int idx = GetTileIdxByPosition( vPos );
            if (idx >= 0)
            {
                int tileX = idx % AutoTileMap.Instance.MapTileWidth;
                int tileY = idx / AutoTileMap.Instance.MapTileWidth;
                return AutoTileMap.Instance.GetAutoTile(tileX, tileY, iLayer);
            }
            return new AutoTileMap.AutoTile() { Idx = -1 };
        }

        /// <summary>
        /// Set a tile using a world position, instead a map coordinate
        /// </summary>
        /// <param name="vPos">World position</param>
        /// <param name="tileIdx">Tile index</param>
        /// <param name="iLayer">0 for ground, 1 for ground overlay, 2 for overlay (see AutoTileMap.eLayerType)</param>
        public static void SetAutoTileByPosition( Vector3 vPos, int tileIdx, int iLayer )
        {
            vPos -= AutoTileMap.Instance.transform.position;
            vPos.y = -vPos.y;// transform to pixel coords
            vPos *= AutoTileset.PixelToUnits;
            if (vPos.x >= 0 && vPos.y >= 0)
            {
                int tile_x = (int)vPos.x / AutoTileMap.Instance.Tileset.TileWidth;
                int tile_y = (int)vPos.y / AutoTileMap.Instance.Tileset.TileWidth;
                AutoTileMap.Instance.SetAutoTile( tile_x, tile_y, tileIdx, iLayer);
            }
        }

        /// <summary>
        /// Return the center world position of a map tile rect
        /// </summary>
        /// <param name="tile_x">x map coordinate</param>
        /// <param name="tile_y">y map coordinate</param>
        /// <returns></returns>
        public static Vector3 GetTileCenterPosition( int tile_x, int tile_y )
        {
            Vector3 vPos = new Vector3((tile_x + 0.5f) * AutoTileMap.Instance.Tileset.TileWorldWidth, -(tile_y + 0.5f) * AutoTileMap.Instance.Tileset.TileWorldHeight);
            return vPos;
        }

        /// <summary>
        /// Draw a debug rect in the editor
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public static void DebugDrawRect( Vector3 pos, Rect rect, Color color )
		{
			rect.position += new Vector2(pos.x, pos.y);
			Debug.DrawLine( new Vector3(rect.x, rect.y, pos.z ), new Vector3(rect.x+rect.width, rect.y, pos.z ), color );
            Debug.DrawLine(new Vector3(rect.x, rect.y, pos.z), new Vector3(rect.x, rect.y + rect.height, pos.z), color);
            Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y, pos.z), new Vector3(rect.x + rect.width, rect.y + rect.height, pos.z), color);
            Debug.DrawLine(new Vector3(rect.x, rect.y + rect.height, pos.z), new Vector3(rect.x + rect.width, rect.y + rect.height, pos.z), color);
		}

        /// <summary>
        /// Draw a debug rect in the editor
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
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
