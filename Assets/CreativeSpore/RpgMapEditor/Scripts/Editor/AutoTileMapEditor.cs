using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CreativeSpore.RpgMapEditor
{
	[CustomEditor(typeof(AutoTileMap))]
	public class AutoTileMapEditor : Editor
	{
		AutoTileMap MyAutoTileMap { get { return (AutoTileMap)target; } }
		bool IsEditModeOn = false;
		Rect m_rTilesetScrollView = new Rect();
		TilesetComponent m_tilesetComponent;

		int m_mapWidth;
		int m_mapHeight;
		bool m_showCollisions = false;

		void OnEnable()
		{
			m_tilesetComponent = new TilesetComponent( MyAutoTileMap );
			if( MyAutoTileMap.MapData != null )
			{
				m_mapWidth = MyAutoTileMap.MapData.Data.TileMapWidth;
				m_mapHeight = MyAutoTileMap.MapData.Data.TileMapHeight;
			}
			if( MyAutoTileMap.BrushGizmo != null )
			{
				MyAutoTileMap.BrushGizmo.gameObject.SetActive(false);
			}
		}

		void OnDisable()
		{
			if( MyAutoTileMap != null )
			{
				if( MyAutoTileMap.BrushGizmo != null )
				{
					MyAutoTileMap.BrushGizmo.gameObject.SetActive(false);
				}

				if( IsEditModeOn )
				{
					Debug.LogWarning(" You forgot to commit map changes! Map will be saved automatically for you! ");
					MyAutoTileMap.SaveMap();
				}
			}
		}

		public void OnSceneGUI()
		{
			if( !MyAutoTileMap.IsInitialized )
			{
				return;
			}

			Handles.Label( HandleUtility.GUIPointToWorldRay(Vector3.zero).origin, " Brush Pos: "+MyAutoTileMap.BrushGizmo.BrushTilePos);
			Handles.Label( HandleUtility.GUIPointToWorldRay(new Vector3(0f, 16f)).origin, " Select Tile Idx: "+m_tilesetComponent.SelectedTileIdx);

            Rect rAutoTileMap = new Rect(MyAutoTileMap.transform.position.x, MyAutoTileMap.transform.position.y, MyAutoTileMap.MapTileWidth * MyAutoTileMap.Tileset.TileWorldWidth, -MyAutoTileMap.MapTileHeight * MyAutoTileMap.Tileset.TileWorldHeight);
			UtilsGuiDrawing.DrawRectWithOutline( rAutoTileMap, new Color(0f, 0f, 0f, 0f), new Color(1f, 1f, 1f, 1f));
			if( m_showCollisions )
			{
				DrawCollisions();
			}

			if (IsEditModeOn)
			{
				int controlID = GUIUtility.GetControlID(FocusType.Passive);
				HandleUtility.AddDefaultControl(controlID);
				EventType currentEventType = Event.current.GetTypeForControl(controlID);
				bool skip = false;
				int saveControl = GUIUtility.hotControl;

                if (currentEventType == EventType.Layout) { skip = true; }
                else if (currentEventType == EventType.ScrollWheel) { skip = true; }

                if (!skip)
                {
                    EditorGUIUtility.AddCursorRect(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), MouseCursor.Arrow);
                    GUIUtility.hotControl = controlID;

                    m_tilesetComponent.OnSceneGUI();

                    if (currentEventType == EventType.MouseDrag && Event.current.button < 2) // 2 is for central mouse button
                    {
                        // avoid dragging the map
                        Event.current.Use();
                    }
                }

				GUIUtility.hotControl = saveControl;

				if (GUI.changed) 
				{
					EditorUtility.SetDirty(target);
				}
			}
		}

        enum eTabType
        {
            Settings,
            Paint,
            Data
        }
        private eTabType m_tabType = eTabType.Settings;

		public override void OnInspectorGUI()
		{
			// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
			serializedObject.Update();

			EditorGUILayout.BeginHorizontal();
			if( MyAutoTileMap.Tileset != null && MyAutoTileMap.Tileset.AtlasTexture == null )
			{
				MyAutoTileMap.Tileset.AtlasTexture = EditorGUILayout.ObjectField ("Tileset Atlas", MyAutoTileMap.Tileset.AtlasTexture, typeof(Texture2D), false) as Texture2D;
				if (GUILayout.Button("Edit Tileset..."))
				{
					AutoTilesetEditorWindow.ShowDialog( MyAutoTileMap.Tileset );
				}
			}
			else
			{
				MyAutoTileMap.Tileset = (AutoTileset)EditorGUILayout.ObjectField("Tileset", MyAutoTileMap.Tileset, typeof(AutoTileset), false);
			}

			if( MyAutoTileMap.Tileset == null )
			{
				if( GUILayout.Button("Create...") )
				{
					MyAutoTileMap.Tileset = RpgMapMakerEditor.CreateTileset();
				}
			}
			EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
			MyAutoTileMap.MapData = (AutoTileMapData)EditorGUILayout.ObjectField("Map Data", MyAutoTileMap.MapData, typeof(AutoTileMapData), false);
			if( MyAutoTileMap.MapData == null && GUILayout.Button("Create...") )
			{
				MyAutoTileMap.MapData = RpgMapMakerEditor.CreateAutoTileMapData();
			}
			EditorGUILayout.EndHorizontal();

			if( MyAutoTileMap.Tileset != null && MyAutoTileMap.MapData != null && MyAutoTileMap.IsInitialized )
			{
                string[] toolBarButtonNames = System.Enum.GetNames(typeof(eTabType));

                m_tabType = (eTabType)GUILayout.Toolbar((int)m_tabType, toolBarButtonNames);
                switch (m_tabType)
                {
                    case eTabType.Settings: DrawSettingsTab(); break;
                    case eTabType.Paint: DrawPaintTab(); break;
                    case eTabType.Data: DrawDataTab(); break;
                }								
			}
            else if (!MyAutoTileMap.IsInitialized)
            {
                MyAutoTileMap.LoadMap();
            }
            else
            {
                EditorGUILayout.HelpBox("You need to select or create a Tileset and a Map Data asset", MessageType.Info);
            }

			if( GUI.changed )
			{
				EditorUtility.SetDirty( MyAutoTileMap );
				if( MyAutoTileMap.Tileset != null )
					EditorUtility.SetDirty( MyAutoTileMap.Tileset );
				if( MyAutoTileMap.MapData != null )
					EditorUtility.SetDirty( MyAutoTileMap.MapData );
			}

			// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
			serializedObject.ApplyModifiedProperties();
		}	

        void DrawSettingsTab()
        {
            MyAutoTileMap.ViewCamera = (Camera)EditorGUILayout.ObjectField("View Camera", MyAutoTileMap.ViewCamera, typeof(Camera), true);
            m_mapWidth = EditorGUILayout.IntField("Map Width", m_mapWidth);
            m_mapHeight = EditorGUILayout.IntField("Map Height", m_mapHeight);
            if (m_mapWidth != MyAutoTileMap.MapData.Data.TileMapWidth || m_mapHeight != MyAutoTileMap.MapData.Data.TileMapHeight)
            {
                if (GUILayout.Button("Apply New Dimensions"))
                {
                    MyAutoTileMap.MapData.Data.TileMapWidth = m_mapWidth;
                    MyAutoTileMap.MapData.Data.TileMapHeight = m_mapHeight;
                    MyAutoTileMap.SaveMap();
                    MyAutoTileMap.LoadMap();
                }
            }

            MyAutoTileMap.AnimatedTileSpeed = EditorGUILayout.FloatField("Animated Tile Speed", MyAutoTileMap.AnimatedTileSpeed);

            MyAutoTileMap.GroundLayerZ = EditorGUILayout.FloatField("GroundLayerZ", MyAutoTileMap.GroundLayerZ);
            MyAutoTileMap.GroundOverlayLayerZ = EditorGUILayout.FloatField("GroundOverlayLayerZ", MyAutoTileMap.GroundOverlayLayerZ);
            MyAutoTileMap.OverlayLayerZ = EditorGUILayout.FloatField("OverlayLayerZ", MyAutoTileMap.OverlayLayerZ);

            MyAutoTileMap.AutoTileMapGui.enabled = EditorGUILayout.Toggle("Show Map Editor On Play", MyAutoTileMap.AutoTileMapGui.enabled);
            if (Application.isPlaying)
            {
                MyAutoTileMap.IsCollisionEnabled = EditorGUILayout.Toggle("Collision Enabled", MyAutoTileMap.IsCollisionEnabled);
            }
            else
            {
                Renderer minimapRenderer = MyAutoTileMap.EditorMinimapRender.GetComponent<Renderer>();
                bool prevEnabled = minimapRenderer.enabled;
                minimapRenderer.enabled = EditorGUILayout.Toggle("Show Minimap", minimapRenderer.enabled);                
                if (!prevEnabled && minimapRenderer.enabled) MyAutoTileMap.RefreshMinimapTexture();
                MyAutoTileMap.BrushGizmo.IsRefreshMinimapEnabled = minimapRenderer.enabled;
            }
            m_showCollisions = EditorGUILayout.Toggle("Show Collisions", m_showCollisions);
            MyAutoTileMap.SaveChangesAfterPlaying = EditorGUILayout.Toggle("Save Changes After Playing", MyAutoTileMap.SaveChangesAfterPlaying);

            //DrawDefaultInspector();
        }

        void DrawDataTab()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (GUILayout.Button("Reload Map"))
            {
                MyAutoTileMap.LoadMap();
            }

            if (GUILayout.Button("Import Map..."))
            {
                if (MyAutoTileMap.ShowLoadDialog())
                {
                    EditorUtility.SetDirty(MyAutoTileMap);
                    SceneView.RepaintAll();
                }
            }

            if (GUILayout.Button("Export Map..."))
            {
                MyAutoTileMap.ShowSaveDialog();
            }

            if (GUILayout.Button("Clear Map..."))
            {
                bool isOk = EditorUtility.DisplayDialog("Clear Map...", "Are you sure?", "Yes");
                if (isOk)
                {
                    MyAutoTileMap.ClearMap();
                    MyAutoTileMap.SaveMap();
                }
            }

            if (GUILayout.Button("Generate Map..."))
            {
                bool isOk = EditorUtility.DisplayDialog("Generate Map...", "Are you sure?", "Yes");
                if (isOk)
                {
                    MyAutoTileMap.ClearMap();

                    float fDiv = 25f;
                    float xf = Random.value * 100;
                    float yf = Random.value * 100;
                    for (int i = 0; i < MyAutoTileMap.MapTileWidth; i++)
                    {
                        for (int j = 0; j < MyAutoTileMap.MapTileHeight; j++)
                        {
                            float fRand = Random.value;
                            float noise = Mathf.PerlinNoise((i + xf) / fDiv, (j + yf) / fDiv);
                            //Debug.Log( "noise: "+noise+"; i: "+i+"; j: "+j );
                            if (noise < 0.3) //water
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 0, (int)AutoTileMap.eTileLayer.GROUND, false);
                            }
                            else if (noise < 0.4) // water plants
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 0, (int)AutoTileMap.eTileLayer.GROUND, false);
                                if (fRand < noise / 3)
                                    MyAutoTileMap.SetAutoTile(i, j, 5, (int)AutoTileMap.eTileLayer.GROUND_OVERLAY, false);
                            }
                            else if (noise < 0.5 && fRand < (1 - noise / 2)) // dark grass
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 32, (int)AutoTileMap.eTileLayer.GROUND, false);
                            }
                            else if (noise < 0.6 && fRand < (1 - 1.2 * noise)) // flowers
                            {
                                //MyAutoTileMap.AddAutoTile( i, j, 24, (int)AutoTileMap.eTileLayer.GROUND);
                                MyAutoTileMap.SetAutoTile(i, j, 144, (int)AutoTileMap.eTileLayer.GROUND, false);
                                MyAutoTileMap.SetAutoTile(i, j, 288 + Random.Range(0, 5), (int)AutoTileMap.eTileLayer.GROUND_OVERLAY, false);
                            }
                            else if (noise < 0.7) // grass
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 144, (int)AutoTileMap.eTileLayer.GROUND, false);
                            }
                            else // mountains
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 33, (int)AutoTileMap.eTileLayer.GROUND, false);
                            }
                        }
                    }
                    //float now, now2;
                    //now = Time.realtimeSinceStartup;

                    //now2 = Time.realtimeSinceStartup;
                    MyAutoTileMap.RefreshAllTiles();
                    //Debug.Log("RefreshAllTiles execution time(ms): " + (Time.realtimeSinceStartup - now2) * 1000);

                    //now2 = Time.realtimeSinceStartup;
                    MyAutoTileMap.SaveMap();
                    //Debug.Log("SaveMap execution time(ms): " + (Time.realtimeSinceStartup - now2) * 1000);

                    MyAutoTileMap.RefreshMinimapTexture();

                    //now2 = Time.realtimeSinceStartup;
                    MyAutoTileMap.UpdateChunks();
                    //Debug.Log("UpdateChunks execution time(ms): " + (Time.realtimeSinceStartup - now2) * 1000);

                    //Debug.Log("Total execution time(ms): " + (Time.realtimeSinceStartup - now) * 1000);

                }
            }
        }

        void DrawPaintTab()
        {
            if (IsEditModeOn)
            {
                if (GUILayout.Button("Commit"))
                {
                    IsEditModeOn = false;
                    if (MyAutoTileMap.BrushGizmo != null)
                    {
                        MyAutoTileMap.BrushGizmo.gameObject.SetActive(false);
                    }                    
                    MyAutoTileMap.SaveMap();
                    EditorUtility.SetDirty(MyAutoTileMap);
                    EditorUtility.SetDirty(MyAutoTileMap.Tileset);
                    EditorUtility.SetDirty(MyAutoTileMap.MapData);
                    AssetDatabase.SaveAssets();
                    Repaint();
                }

                if (Event.current.type == EventType.Repaint)
                {
                    m_rTilesetScrollView = GUILayoutUtility.GetLastRect();
                    m_rTilesetScrollView.y = m_rTilesetScrollView.yMax + 32;
                    m_rTilesetScrollView.height = Mathf.Max(512, Screen.height - m_rTilesetScrollView.yMax - 16);
                }
                GUILayout.Space(m_rTilesetScrollView.height + 32);
                m_tilesetComponent.OnInspectorGUI(m_rTilesetScrollView);
                m_tilesetComponent.IsEditCollision = Event.current.shift;

                Repaint();

            }
            else
            {
                GUILayout.BeginVertical();
                if (GUILayout.Button("Edit"))
                {
                    IsEditModeOn = true;
                    if (MyAutoTileMap.BrushGizmo != null)
                    {
                        MyAutoTileMap.BrushGizmo.gameObject.SetActive(true);
                    }
                    Repaint();
                }
                EditorGUILayout.HelpBox("Press Edit to start painting and remember to commit your changes to be sure they are saved into asset map data.", MessageType.Info);
                GUILayout.EndVertical();
            }
        }

		void DrawCollisions()
		{
            float fCollW = MyAutoTileMap.Tileset.TileWorldWidth / 4;
            float fCollH = MyAutoTileMap.Tileset.TileWorldHeight / 4;
			Rect rColl = new Rect(0, 0, fCollW, -fCollH);
			Color cColl = new Color( 1f, 0f, 0f, 0.1f );
			Vector3 vTopLeft = HandleUtility.GUIPointToWorldRay(Vector3.zero).origin;
			Vector3 vBottomRight = HandleUtility.GUIPointToWorldRay( new Vector3( Screen.width, Screen.height ) ).origin;
			vTopLeft.y = -vTopLeft.y;
			vBottomRight.y = -vBottomRight.y;
			vTopLeft.x -= (vTopLeft.x % fCollW) + fCollW/2;
			vTopLeft.y -= (vTopLeft.y % fCollH) + fCollH/2;
			vBottomRight.x -= (vBottomRight.x % fCollW) - fCollW/2;
			vBottomRight.y -= (vBottomRight.y % fCollH) - fCollH/2;
            for (float y = vTopLeft.y; y <= vBottomRight.y; y += MyAutoTileMap.Tileset.TileWorldHeight / 4)
			{
                for (float x = vTopLeft.x; x <= vBottomRight.x; x += MyAutoTileMap.Tileset.TileWorldWidth / 4)
				{
					AutoTileMap.eTileCollisionType collType = MyAutoTileMap.GetAutotileCollisionAtPosition( new Vector3( x, -y ) );
					if( collType != AutoTileMap.eTileCollisionType.PASSABLE )
					{
						rColl.position = new Vector2(x-fCollW/2, -(y-fCollH/2));
						UtilsGuiDrawing.DrawRectWithOutline( rColl, cColl, cColl );
					}
				}
			}
		}
	}
}