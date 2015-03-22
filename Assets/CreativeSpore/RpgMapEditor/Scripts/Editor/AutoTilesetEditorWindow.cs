using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CreativeSpore.RpgMapEditor
{
	public class AutoTilesetEditorWindow : EditorWindow 
	{
		AutoTileset m_autoTileset;
		AutoTileMap.eTilesetGroupType m_tilesetGroupType;
		Texture2D m_tilesetTexture;
		Vector2 m_scrollPos;

		public static void ShowDialog( AutoTileset _autoTileset )
		{
			AutoTilesetEditorWindow window = (AutoTilesetEditorWindow)EditorWindow.GetWindow (typeof (AutoTilesetEditorWindow));
			window.m_autoTileset = _autoTileset;
			window.wantsMouseMove = true;
		}

		void OnGUI () 
		{
			GUILayout.Label ("Tileset Collision Configuration", EditorStyles.boldLabel);

			if( m_autoTileset.TilesetsAtlasTexture == null )
			{
				Selection.activeObject = m_autoTileset;
				GUILayout.Label("You have to create a texture atlas first");
				return;
			}

			GUILayout.BeginHorizontal();

			if( GUILayout.Button( "Default Configuration") )
			{
				bool isOk = EditorUtility.DisplayDialog("Set Default Collision Data", "Are you sure?", "Yes");
				if( isOk )
				{
					SetDefaultConfig();
					EditorUtility.SetDirty( m_autoTileset );
				}
			}

			if( GUILayout.Button( "Clear") )
			{
				bool isOk = EditorUtility.DisplayDialog("Clear Collision Data", "Are you sure?", "Yes");
				if( isOk )
				{
					System.Array.Clear( m_autoTileset.AutotileCollType, 0, m_autoTileset.AutotileCollType.Length);
					EditorUtility.SetDirty( m_autoTileset );
				}
			}

			GUILayout.EndHorizontal();

			EditorGUILayout.HelpBox( "Press left or right mouse button over the tiles to change the collision type", MessageType.Info);

			m_tilesetGroupType = (AutoTileMap.eTilesetGroupType)EditorGUILayout.EnumPopup( "Tileset: ", m_tilesetGroupType );

			if( GUI.changed || m_tilesetTexture == null )
			{
				m_tilesetTexture = UtilsAutoTileMap.GenerateTilesetTexture( m_autoTileset.TilesetsAtlasTexture, m_tilesetGroupType );
			}

			float fScrollBarWidth = 16f;
			Rect rTileset = new Rect( 0f, 0f, (float)m_tilesetTexture.width, (float)m_tilesetTexture.height);
			Rect rScrollView = new Rect( 50, 120, m_tilesetTexture.width+fScrollBarWidth, position.height - 130 );
			if( m_tilesetTexture != null )
			{
				Rect rTile = new Rect(0, 0, AutoTileset.TileWidth, AutoTileset.TileHeight);
				// BeginScrollView
				m_scrollPos = GUI.BeginScrollView( rScrollView, m_scrollPos, rTileset);
				{
					GUI.DrawTexture( rTileset, m_tilesetTexture );

					for( int autoTileLocalIdx = 0; autoTileLocalIdx < 256; ++autoTileLocalIdx ) //autoTileLocalIdx: index of current tileset group
					{
						rTile.x = rTileset.x + (autoTileLocalIdx % AutoTileset.AutoTilesPerRow) * AutoTileset.TileWidth;
						rTile.y = rTileset.y + (autoTileLocalIdx / AutoTileset.AutoTilesPerRow) * AutoTileset.TileHeight;

						int autoTileIdx = autoTileLocalIdx + (int)m_tilesetGroupType * 256; // global autotile idx
						if (Event.current.type == EventType.MouseUp)
						{
							if( rTile.Contains( Event.current.mousePosition ) )
							{
								int collType = (int)m_autoTileset.AutotileCollType[ autoTileIdx ];
								if( Event.current.button == 0 )
								{
									collType += 1; // go next
								}
								else if( Event.current.button == 1 )
								{
									collType += (int)AutoTileMap.eTileCollisionType._SIZE - 1; // go back
								}
								collType%=(int)AutoTileMap.eTileCollisionType._SIZE;
								m_autoTileset.AutotileCollType[ autoTileIdx ] = (AutoTileMap.eTileCollisionType)(collType);
							}
							EditorUtility.SetDirty( m_autoTileset );
						}

						string sCollision = "";
						switch( m_autoTileset.AutotileCollType[autoTileIdx] )
						{
						case AutoTileMap.eTileCollisionType.BLOCK: sCollision = "■"; break;
						case AutoTileMap.eTileCollisionType.FENCE: sCollision = "#"; break;
						case AutoTileMap.eTileCollisionType.WALL: sCollision = "□"; break;
						case AutoTileMap.eTileCollisionType.OVERLAY: sCollision = "★"; break;
						}
						if( sCollision.Length > 0 )
						{
							GUI.color = new Color(1f, 1f, 1f, 1f);
							GUIStyle style = new GUIStyle();
							style.fontSize = 30;
							style.fontStyle = FontStyle.Bold;
							style.alignment = TextAnchor.MiddleCenter;
							style.normal.textColor = Color.white;
							GUI.Box( rTile, sCollision, style );
							GUI.color = Color.white;
						}

						//Show alpha tiles (debug)
						/*
						if( m_autoTileset.IsAutoTileHasAlpha[autoTileIdx] )
						{
							GUIStyle style = new GUIStyle();
							style.fontSize = 30;
							style.fontStyle = FontStyle.Bold;
							style.alignment = TextAnchor.MiddleCenter;
							style.normal.textColor = Color.blue;
							GUI.Box( rTile, "A", style );
						}
						*/
					}
				}
				GUI.EndScrollView();

				// Help Info
				{
					GUIStyle style = new GUIStyle();
					style.fontSize = 15;
					style.fontStyle = FontStyle.Bold;
					//NOTE: if you don't see the special characters properly, be sure this file is saved in UTF-8
					GUI.Label( new Rect( rScrollView.xMax + 30, rScrollView.y + 40, 100, 100), "■ - Block Collision", style);
					GUI.Label( new Rect( rScrollView.xMax + 30, rScrollView.y + 60, 100, 100), "□ - Wall Collision", style);
					GUI.Label( new Rect( rScrollView.xMax + 30, rScrollView.y + 80, 100, 100), "# - Fence Collision", style);
					GUI.Label( new Rect( rScrollView.xMax + 30, rScrollView.y + 100, 100, 100), "★ - Overlay", style);
				}

				Repaint();
			}
		}

		void SetDefaultConfig()
		{
			// Set animated collisions
			for( int i = 0; i < 16; ++i )
			{
				m_autoTileset.AutotileCollType[i] = AutoTileMap.eTileCollisionType.WALL;
			}

			// Set building collision
			for( int i = 48; i < 80; ++i )
			{
				m_autoTileset.AutotileCollType[i] = AutoTileMap.eTileCollisionType.BLOCK;
			}
			// Set wall collision
			for( int i = 80; i < 128; ++i )
			{
				m_autoTileset.AutotileCollType[i] = ( (i/AutoTileset.AutoTilesPerRow)%2 == 0 )? AutoTileMap.eTileCollisionType.WALL : AutoTileMap.eTileCollisionType.BLOCK;
			}
		}
	}
}