using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CreativeSpore.RpgMapEditor
{
	public class AutoTileBrush : MonoBehaviour 
	{

		public AutoTileMap MyAutoTileMap;

		public TileAction BrushAction; // tile action with copied tiles to be pasted over the map

		public Vector2 BrushTilePos;
		public bool HasChangedTilePos;

		#region Historic Ctrl-Z Ctrl-Y
		[System.Serializable]
		public class TileAction
		{
			public class TileData
			{
				public int Tile_x;
				public int Tile_y;
				public int Tile_type;
				public int Tile_layer;
				public int Tile_type_prev;
			}
			
			List<TileData> aTileData = new List<TileData>();
			
			public void Push( AutoTileMap _autoTileMap, int tile_x, int tile_y, int tile_type, int tile_layer )
			{
				if( tile_type <= -2 )
				{
					; // do nothing, skip tile
				}
				else
				{
					if( tile_type >= 0 && _autoTileMap.Tileset.AutotileCollType[ tile_type ] == AutoTileMap.eTileCollisionType.OVERLAY )
					{
						tile_layer = (int)AutoTileMap.eTileLayer.OVERLAY;
					}
					
					TileData tileData = new TileData()
					{  
						Tile_x = tile_x,
						Tile_y = tile_y,
						Tile_type = tile_type,
						Tile_layer = tile_layer,
					};
					aTileData.Add( tileData );
				}
			}
			
			public void DoAction( AutoTileMap _autoTileMap )
			{
				int tileMinX = _autoTileMap.MapTileWidth-1;
				int tileMinY = _autoTileMap.MapTileHeight-1;
				int tileMaxX = 0;
				int tileMaxY = 0;

				for( int i = 0; i < aTileData.Count; ++i )
				{
					TileData tileData = aTileData[i];
					// save prev tile type for undo action
					tileData.Tile_type_prev = _autoTileMap.GetAutoTile( tileData.Tile_x, tileData.Tile_y, tileData.Tile_layer ).Type;
					_autoTileMap.SetAutoTile( tileData.Tile_x, tileData.Tile_y, tileData.Tile_type, tileData.Tile_layer );

					tileMinX = Mathf.Min( tileMinX, tileData.Tile_x );
					tileMinY = Mathf.Min( tileMinY, tileData.Tile_y );
					tileMaxX = Mathf.Max( tileMaxX, tileData.Tile_x );
					tileMaxY = Mathf.Max( tileMaxY, tileData.Tile_y );
				}
				_autoTileMap.RefreshMinimapTexture( tileMinX, tileMinY, (tileMaxX-tileMinX)+1, (tileMaxY-tileMinY)+1 );
				_autoTileMap.UpdateChunks();
			}
			
			public void UndoAction( AutoTileMap _autoTileMap )
			{
				int tileMinX = _autoTileMap.MapTileWidth-1;
				int tileMinY = _autoTileMap.MapTileHeight-1;
				int tileMaxX = 0;
				int tileMaxY = 0;
				
				for( int i = 0; i < aTileData.Count; ++i )
				{
					TileData tileData = aTileData[i];
					_autoTileMap.SetAutoTile( tileData.Tile_x, tileData.Tile_y, tileData.Tile_type_prev, tileData.Tile_layer );

					tileMinX = Mathf.Min( tileMinX, tileData.Tile_x );
					tileMinY = Mathf.Min( tileMinY, tileData.Tile_y );
					tileMaxX = Mathf.Max( tileMaxX, tileData.Tile_x );
					tileMaxY = Mathf.Max( tileMaxY, tileData.Tile_y );
				}
				_autoTileMap.RefreshMinimapTexture( tileMinX, tileMinY, (tileMaxX-tileMinX)+1, (tileMaxY-tileMinY)+1 );
				_autoTileMap.UpdateChunks();
			}
			
			public void CopyRelative( AutoTileMap _autoTileMap, TileAction _action, int tile_x, int tile_y )
			{
				foreach( TileData tileData in _action.aTileData )
				{
					Push( _autoTileMap, tileData.Tile_x + tile_x, tileData.Tile_y + tile_y, tileData.Tile_type, tileData.Tile_layer );
				}
			}
			
			// tiles in ground will be moved to overlay
			public void BecomeOverlay()
			{
				for( int idx = 0; idx < aTileData.Count; ++idx )
				{
					TileData tileData = aTileData[idx];
					if( tileData.Tile_layer == (int)AutoTileMap.eTileLayer.GROUND )
					{
						tileData.Tile_layer = (int)AutoTileMap.eTileLayer.GROUND_OVERLAY;
					}
					if( tileData.Tile_layer == (int)AutoTileMap.eTileLayer.GROUND_OVERLAY && tileData.Tile_type == -1 )
					{
						aTileData.RemoveAt(idx);
						--idx;
					}
				}
			}
			
		}
		
		private int m_actionIdx = -1;
		private List<TileAction>  m_actionsHistoric = new List<TileAction>();
		
		public void PerformAction( TileAction _action )
		{
			if( m_actionIdx < (m_actionsHistoric.Count - 1) && m_actionsHistoric.Count > 0 )
			{
				m_actionsHistoric.RemoveRange( m_actionIdx+1, m_actionsHistoric.Count - m_actionIdx - 1 );
			}
			
			m_actionsHistoric.Add( _action ); ++m_actionIdx;
			_action.DoAction( MyAutoTileMap );
		}
		
		public void UndoAction()
		{
			// this could happen in editor mode. Should be fixed by serializing TileAction class, but just in case...
			if( m_actionIdx >= m_actionsHistoric.Count )
			{
				Debug.LogWarning(" AutoTileBrush.UndoAction: m_actionIdx >= m_actionsHistoric.Count will be set to m_actionsHistoric.Count - 1 ");
				m_actionIdx = m_actionsHistoric.Count - 1;
			}

			if( m_actionIdx >= 0 )
			{
				m_actionsHistoric[ m_actionIdx ].UndoAction( MyAutoTileMap );
				--m_actionIdx;
			}
		}
		
		public void RedoAction()
		{
			if( m_actionIdx < m_actionsHistoric.Count - 1 )
			{
				++m_actionIdx;
				m_actionsHistoric[ m_actionIdx ].DoAction( MyAutoTileMap );
			}
		}
		#endregion

		public void UpdateBrushGizmo( Vector3 mousePos )
		{
			Vector3 vTemp = mousePos;
			vTemp.x -= mousePos.x % AutoTileset.TileWorldWidth;
			vTemp.y -= mousePos.y % AutoTileset.TileWorldHeight;
			vTemp.z += 1f;
			transform.position = vTemp;

			int tile_x = (int)( 0.5 + transform.position.x / AutoTileset.TileWorldWidth);
			int tile_y = (int)( 0.5 + -transform.position.y / AutoTileset.TileWorldHeight);

			Vector2 vPrevTilePos = BrushTilePos;
			BrushTilePos = new Vector2( tile_x, tile_y );
			HasChangedTilePos = (vPrevTilePos != BrushTilePos);
		}

		public void Clear()
		{
			RefreshBrushGizmo( -1, -1, -1, -1, -1, -1, false );
			BrushAction = null;
		}

		public void RefreshBrushGizmo( int tile_start_x, int tile_start_y, int tile_end_x, int tile_end_y, int _dragEndTileX, int _dragEndTileY, bool isCtrlKeyHold )
		{

			SpriteRenderer[] aSprites = GetComponentsInChildren<SpriteRenderer>();
			
			int sprIdx = 0;
			for( int tile_x = tile_start_x; tile_x <= tile_end_x; ++tile_x)
			{
				for( int tile_y = tile_start_y; tile_y <= tile_end_y; ++tile_y)
				{
					for( int tile_layer = 0; tile_layer < (int)AutoTileMap.eTileLayer._SIZE; ++tile_layer )
					{
						if( isCtrlKeyHold && tile_layer == (int)AutoTileMap.eTileLayer.GROUND )
						{
							//copy overlay only
							continue;
						}
						
						AutoTileMap.AutoTile autoTile = MyAutoTileMap.GetAutoTile( tile_x, tile_y, tile_layer );
						if( autoTile != null && autoTile.TilePartsIdx != null )
						{
							for( int partIdx = 0; partIdx < autoTile.TilePartsLength; ++partIdx, ++sprIdx )
							{
								SpriteRenderer spriteRender = sprIdx < aSprites.Length? aSprites[sprIdx] : null;
								if( spriteRender == null )
								{
									GameObject spriteObj = new GameObject();
									spriteObj.transform.parent = transform;
									spriteRender = spriteObj.AddComponent<SpriteRenderer>();
								}
								spriteRender.transform.gameObject.name = "BrushGizmoPart"+sprIdx;
								spriteRender.sprite = MyAutoTileMap.Tileset.AutoTileSprites[ autoTile.TilePartsIdx[ partIdx ] ];
								spriteRender.sortingOrder = 50; //TODO: +50 temporal? see for a const number later
								spriteRender.color = new Color32( 192, 192, 192, 192);

								// get last tile as relative position
								int tilePart_x = (tile_x - _dragEndTileX)*2 + partIdx%2;
								int tilePart_y = (tile_y - _dragEndTileY)*2 + partIdx/2;
								
								float xFactor = AutoTileset.TilePartWidth/AutoTileset.PixelToUnits;
								float yFactor = AutoTileset.TilePartHeight/AutoTileset.PixelToUnits;
								spriteRender.transform.localPosition = new Vector3( tilePart_x * xFactor, -tilePart_y * yFactor, spriteRender.transform.position.z );
							}
						}
					}
				}
			}
			// clean unused sprite objects
			while(sprIdx < aSprites.Length)
			{
				if( Application.isEditor )
				{
					DestroyImmediate( aSprites[sprIdx].transform.gameObject );
				}
				else
				{
					Destroy( aSprites[sprIdx].transform.gameObject );
				}
				++sprIdx;
			}
		}
		
		public void RefreshBrushGizmoFromTileset( int tilesetSelStart, int tilesetSelEnd )
		{
			SpriteRenderer[] aSprites = GetComponentsInChildren<SpriteRenderer>();
			
			BrushAction = new TileAction();
			
			int selTileW = (Mathf.Abs( tilesetSelStart - tilesetSelEnd )% AutoTileset.AutoTilesPerRow + 1);
			int selTileH = (Mathf.Abs( tilesetSelStart - tilesetSelEnd )/ AutoTileset.AutoTilesPerRow + 1);
			int tilesetIdx = Mathf.Min( tilesetSelStart, tilesetSelEnd );
			int sprIdx = 0;
			Vector3 vSprPos = new Vector3( 0f, 0f, 0f );
			for( int j = 0; j < selTileH; ++j, tilesetIdx += (AutoTileset.AutoTilesPerRow-selTileW), vSprPos.y -= AutoTileset.TileWorldHeight )
			{
				vSprPos.x = 0;
				for( int i = 0; i < selTileW; ++i, ++tilesetIdx, ++sprIdx, vSprPos.x += AutoTileset.TileWorldWidth )
				{
					SpriteRenderer spriteRender = sprIdx < aSprites.Length? aSprites[sprIdx] : null;
					if( spriteRender == null )
					{
						GameObject spriteObj = new GameObject();
						spriteObj.transform.parent = transform;
						spriteRender = spriteObj.AddComponent<SpriteRenderer>();
					}
					spriteRender.transform.gameObject.name = "BrushGizmoPart"+sprIdx;
					spriteRender.sprite = MyAutoTileMap.Tileset.ThumbnailSprites[ tilesetIdx ];
					spriteRender.sortingOrder = 50; //TODO: +50 temporal? see for a const number later
					spriteRender.color = new Color32( 255, 255, 255, 160);
					
					spriteRender.transform.localPosition = vSprPos;
					
					bool hasAlpha = MyAutoTileMap.IsAutoTileHasAlpha( tilesetIdx );
					BrushAction.Push( MyAutoTileMap, i, j, tilesetIdx,  (int)( hasAlpha? AutoTileMap.eTileLayer.GROUND_OVERLAY : AutoTileMap.eTileLayer.GROUND));
				}
			}
			
			// clean unused sprite objects
			while(sprIdx < aSprites.Length)
			{
				if( Application.isEditor )
				{
					DestroyImmediate( aSprites[sprIdx].transform.gameObject );
				}
				else
				{
					Destroy( aSprites[sprIdx].transform.gameObject );
				}
				++sprIdx;
			}
		}
	}
}
