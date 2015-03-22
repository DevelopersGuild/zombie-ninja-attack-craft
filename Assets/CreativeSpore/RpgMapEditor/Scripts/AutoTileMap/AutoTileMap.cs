using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreativeSpore.RpgMapEditor
{

	[RequireComponent(typeof(AutoTileMapGui))]
	[RequireComponent(typeof(AutoTileMapEditorBehaviour))]
	public class AutoTileMap : MonoBehaviour 
	{
		public static AutoTileMap Instance{ get; private set; }

		public class AutoTile
		{
			public int Type = -1;	// tile idx dependent of tile type ( the animated tileset has some tiles grouped as one and the idx is different )
			public int AutoTileIdx; // real tile idx independent of tileset type ( this is used for graphical part with sprites )
			public int TilesetIdx; // tileset idx owned of this autotile
			public int TileX;
			public int TileY;
			public int Layer;
			public int[] TilePartsIdx;
			public eTilePartType[] TilePartsType;
			public int TilePartsLength; // added to specify the length of TileParts. Usually 4, but only 1 for OBJECT and NORMAL tiles

            public bool IsWaterTile()
            {
                return Type != -1 && TilesetIdx == 0; // TODO: temporary fix: if it's an animated tileset, it's considered as water
            }
		};

		public enum eTilePartType
		{
			INT_CORNER,
			EXT_CORNER,
			INTERIOR,
			H_SIDE, // horizontal sides
			V_SIDE // vertical sides
		}

		public enum eTileLayer
		{
			GROUND,			// mostly for tiles with no alpha
			GROUND_OVERLAY, // mostly for tiles with alpha
			OVERLAY,	// for tiles that should be drawn over everything else
			_SIZE,
		}

		public enum eTilesetType
		{
			ANIMATED,
			GROUND,
			BUILDINGS,
			WALLS,
			NORMAL,
			OBJECTS
		};

		public enum eTileCollisionType
		{
			EMPTY = -1, // used to indicate the empty tile with no type
            PASSABLE, //NOTE: there are PASSABLE and EMPTY for collisions. A PASSABLE tile over a BLOC, WALL, or FENCE allow walking over it.
			BLOCK,
			WALL,
			FENCE,
			OVERLAY,
			_SIZE
		}

		public enum eTilesetGroupType
		{
			GROUND,
			OBJECTS_B,
			OBJECTS_C,
			OBJECTS_D,
			OBJECTS_E
		}

		public AutoTileset Tileset;

		[SerializeField]
		AutoTileMapData m_mapData;
		public AutoTileMapData MapData
		{ 
			get{ return m_mapData; } 
			set
			{
				bool isChanged = m_mapData != value;
				m_mapData = value;
				if( isChanged )
				{
					LoadMap();
				}
			}
		}

		[SerializeField]
		AutoTileBrush m_brushGizmo;
		public AutoTileBrush BrushGizmo
		{
			get
			{
				if( m_brushGizmo == null )
				{
					GameObject objBrush = new GameObject();
					objBrush.name = "BrushGizmo";
					objBrush.transform.parent = transform;
					m_brushGizmo = objBrush.AddComponent<AutoTileBrush>();
					m_brushGizmo.MyAutoTileMap = this;
				}
				return m_brushGizmo;
			}	
		}

		public SpriteRenderer EditorMinimapRender;

		public Texture2D MinimapTexture{ get; private set; }

		public int MapTileWidth		{ get{ return MapData != null? MapData.Data.TileMapWidth : 0; } }
		public int MapTileHeight 	{ get{ return MapData != null? MapData.Data.TileMapHeight : 0; } }

		public Camera ViewCamera;
		public AutoTileMapGui AutoTileMapGui{ get; private set; }

		public List<Vector3> TileLayerPosition = new List<Vector3>()
		{ 
			new Vector3( 0f, 0f, +1f ),	// GROUND LAYER
			new Vector3( 0f, 0f, +.5f ),// GOUND OVERLAY LAYER
			new Vector3( 0f, 0f, -1f ),// OVERLAY LAYER
		};

		public float GroundLayerZ
		{
			get{ return TileLayerPosition[ (int)eTileLayer.GROUND ].z;}
			set
			{ 
				Vector3 vPos = TileLayerPosition[ (int)eTileLayer.GROUND ]; 
				vPos.z = value; TileLayerPosition[ (int)eTileLayer.GROUND ] = vPos; 
				m_tileChunkPoolNode.UpdateLayerPositions();
			}
		}

		public float GroundOverlayLayerZ
		{
			get{ return TileLayerPosition[ (int)eTileLayer.GROUND_OVERLAY ].z;}
			set
			{ 
				Vector3 vPos = TileLayerPosition[ (int)eTileLayer.GROUND_OVERLAY ]; 
				vPos.z = value; TileLayerPosition[ (int)eTileLayer.GROUND_OVERLAY ] = vPos; 
				m_tileChunkPoolNode.UpdateLayerPositions();
			}
		}

		public float OverlayLayerZ
		{
			get{ return TileLayerPosition[ (int)eTileLayer.OVERLAY ].z;}
			set
			{ 
				Vector3 vPos = TileLayerPosition[ (int)eTileLayer.OVERLAY ]; 
				vPos.z = value; TileLayerPosition[ (int)eTileLayer.OVERLAY ] = vPos;
                vPos.z -= .1f; EditorMinimapRender.transform.position = vPos; // set editor minimap position a little over the overlay layer
				m_tileChunkPoolNode.UpdateLayerPositions();
			}
		}

		public float AnimatedTileSpeed = 6f;

		public bool IsCollisionEnabled = true;

		public bool IsInitialized{ get{ return m_AutoTileLayers != null; } }

		private bool m_isVisible = true;
		public bool IsVisible
		{
			get{ return m_isVisible; }
			set
			{
				m_isVisible = value;
			}
		}

		public bool SaveChangesAfterPlaying = true;

		public int TileAnim3Frame{ get{ return (int)m_tileAnim3Frame; } }
        public int TileAnim4Frame { get { return (int)m_tileAnim4Frame; } }
		public bool TileAnimFrameHasChanged{ get; private set; }

		//NOTE: changing this array will break the asset
		private eTilesetType[] m_tilemapTypes = new eTilesetType[]
		{
			eTilesetType.ANIMATED,
			eTilesetType.GROUND,
			eTilesetType.BUILDINGS,
			eTilesetType.WALLS,
			eTilesetType.NORMAL,
			eTilesetType.OBJECTS,
			eTilesetType.OBJECTS,
			eTilesetType.OBJECTS,
			eTilesetType.OBJECTS,
		};

		private Texture2D m_minimapTilesTexture;

		private List<AutoTile[,]> m_AutoTileLayers;// The auto tile layers. Sorting drawing depends on layer position. Lower the deeper.

		private float m_tileAnim3Frame = 0f;
        private float m_tileAnim4Frame = 0f;

		[SerializeField]
		private TileChunkPool m_tileChunkPoolNode;

		void Awake()
		{
			if( Instance == null )
			{
				DontDestroyOnLoad(gameObject);
				Instance = this;

				if( CanBeInitialized() )
				{
					if( Application.isPlaying && ViewCamera && ViewCamera.name == "SceneCamera" )
					{
						ViewCamera = null;
					}
					LoadMap();
					BrushGizmo.Clear();
					IsVisible = true;
				}
				else
				{
					Debug.LogWarning(" Autotilemap cannot be initialized because Tileset and/or Map Data is missing. Press create button in the inspector to create the missing part or select one.");
				}
			}
			else if( Instance != this )
			{
				Destroy( transform.gameObject );
			}		
		}

		void OnDisable()
		{
			if( IsInitialized && SaveChangesAfterPlaying )
			{
				SaveMap();
				string xml = MapData.Data.GetXmlString( );
				PlayerPrefs.SetString("OnPlayXmlMapData", xml);
			}
		}

		void OnDestroy()
		{
			if( m_brushGizmo != null )
			{
	#if UNITY_EDITOR
				DestroyImmediate(m_brushGizmo.gameObject);
	#else
				Destroy(m_brushGizmo.gameObject);
	#endif
			}
		}

		public void UpdateChunks()
		{
			m_tileChunkPoolNode.UpdateChunks();
		}

		public void LoadMap()
		{
			//Debug.Log("AutoTileMap:LoadMap");

			if( Tileset == null )
			{
				Debug.LogWarning( " AutoTileMap does not have a Tileset yet! " );
				return;
			}

			if( MapData != null )
			{
				if( Application.isEditor )
				{
					string xml = PlayerPrefs.GetString("OnPlayXmlMapData", "");
					PlayerPrefs.SetString("OnPlayXmlMapData", "");
					if( !string.IsNullOrEmpty(xml) && SaveChangesAfterPlaying )
					{
						AutoTileMapSerializeData mapData = AutoTileMapSerializeData.LoadFromXmlString( xml );
						MapData.Data.CopyData( mapData );
	#if UNITY_EDITOR
						EditorUtility.SetDirty( MapData );
						AssetDatabase.SaveAssets();
	#endif
					}
				}
				MapData.Data.LoadToMap( this );
			}
			else
			{
				Initialize();
			}

			m_tileChunkPoolNode.UpdateChunks();
		}
		
		public bool SaveMap()
		{
			//Debug.Log("AutoTileMap:SaveMap");
			return MapData.Data.SaveData( this );
		}

		public bool ShowLoadDialog()
		{
	#if UNITY_EDITOR
			string filePath = EditorUtility.OpenFilePanel( "Load tilemap",	"", "xml");
			if( filePath.Length > 0 )
			{
				AutoTileMapSerializeData mapData = AutoTileMapSerializeData.LoadFromFile( filePath );
				MapData.Data.CopyData( mapData );
				LoadMap();
				return true;
			}
	#else
			string xml = PlayerPrefs.GetString("XmlMapData", "");
			if( !string.IsNullOrEmpty(xml) )
			{
				AutoTileMapSerializeData mapData = AutoTileMapSerializeData.LoadFromXmlString( xml );
				MapData.Data.CopyData( mapData );
				LoadMap();
				
				return true;
			}
	#endif
			return false;
		}

		public void ShowSaveDialog()
		{
	#if UNITY_EDITOR
			string filePath = EditorUtility.SaveFilePanel( "Save tilemap",	"",	"map" + ".xml",	"xml");
			if( filePath.Length > 0 )
			{
				SaveMap();
				MapData.Data.SaveToFile( filePath );
			}
	#else
			SaveMap();
			string xml = MapData.Data.GetXmlString( );
			PlayerPrefs.SetString("XmlMapData", xml);
	#endif
		}

		public bool CanBeInitialized()
		{
			return Tileset != null && Tileset.TilesetsAtlasTexture != null && MapData != null;
		}

		public void Initialize()
		{
			//Debug.Log("AutoTileMap:Initialize");

			if( MapData == null )
			{
				Debug.LogError(" AutoTileMap.Initialize called when MapData was null");
			}
			else if( Tileset == null || Tileset.TilesetsAtlasTexture == null )
			{
				Debug.LogError(" AutoTileMap.Initialize called when Tileset or Tileset.TilesetsAtlasTexture was null");
			}
			else
			{
				//TODO: find a way to serialize List<Sprite> to avoid call GenerateAutoTileData
				Tileset.GenerateAutoTileData();

				MinimapTexture = new Texture2D(MapTileWidth, MapTileHeight);
				MinimapTexture.anisoLevel = 0;
				MinimapTexture.filterMode = FilterMode.Point;
				MinimapTexture.name = "MiniMap";

				m_minimapTilesTexture = new Texture2D( 64, 64 );
				m_minimapTilesTexture.anisoLevel = 0;
				m_minimapTilesTexture.filterMode = FilterMode.Point;
				m_minimapTilesTexture.name = "MiniMapTiles";
				
				_GenerateMinimapTilesTexture();

				if( Application.isEditor )
				{
					if( EditorMinimapRender == null )
					{
						GameObject objMinimap = new GameObject();
						objMinimap.name = "Minimap";
						objMinimap.transform.parent = transform;
						EditorMinimapRender = objMinimap.AddComponent<SpriteRenderer>();
						EditorMinimapRender.GetComponent<Renderer>().enabled = false;
					}
					Rect rMinimap = new Rect(0f, 0f, MinimapTexture.width, MinimapTexture.height);
					Vector2 pivot = new Vector2(0f, 1f);
					EditorMinimapRender.sprite = Sprite.Create(MinimapTexture, rMinimap, pivot, AutoTileset.PixelToUnits);
					EditorMinimapRender.transform.localScale = new Vector3( AutoTileset.TileWidth, AutoTileset.TileHeight );
				}
				
				m_AutoTileLayers = new List<AutoTile[,]>( (int)eTileLayer._SIZE );
				for( int iLayer = 0; iLayer <  (int)eTileLayer._SIZE; ++iLayer )
				{
					m_AutoTileLayers.Add( new AutoTile[MapTileWidth, MapTileHeight] );
					for (int i = 0; i < MapTileWidth; ++i)
					{
						for (int j = 0; j < MapTileHeight; ++j)
						{
							m_AutoTileLayers[iLayer][i, j] = null;
						}
					}
				}
				
				AutoTileMapGui = GetComponent<AutoTileMapGui>();

				if( m_tileChunkPoolNode == null )
				{
					string nodeName = name+" Data";
					GameObject obj = GameObject.Find( nodeName );
					if( obj == null ) obj = new GameObject();
					obj.name = nodeName;
					obj.AddComponent<TileChunkPool>();
					m_tileChunkPoolNode = obj.AddComponent<TileChunkPool>();
				}
				m_tileChunkPoolNode.Initialize( this );
			}
		}

		public void ClearMap()
		{
			if( m_AutoTileLayers != null )
			{
				foreach( AutoTile[,] aAutoTiles in m_AutoTileLayers )
				{
					for (int i = 0; i < MapTileWidth; ++i)
					{
						for (int j = 0; j < MapTileHeight; ++j)
						{
							aAutoTiles[i, j] = null;
						}
					}
				}
			}
            // remove all tile chunks
            m_tileChunkPoolNode.Initialize(this);
		}

		//public int _Debug_SpriteRenderCounter = 0; //debug
		private int __prevTileAnimFrame = -1;
		void Update () 
		{
			if( !IsInitialized )
			{
				return;
			}

			BrushGizmo.gameObject.SetActive( AutoTileMapGui.enabled );

            m_tileAnim4Frame += Time.deltaTime * AnimatedTileSpeed;
            while (m_tileAnim4Frame >= 4f) m_tileAnim4Frame -= 4f;
			m_tileAnim3Frame += Time.deltaTime * AnimatedTileSpeed;
			while( m_tileAnim3Frame >= 3f ) m_tileAnim3Frame -= 3f;
			TileAnimFrameHasChanged = (int)m_tileAnim3Frame != __prevTileAnimFrame ;
			__prevTileAnimFrame = (int)m_tileAnim3Frame;	

			m_tileChunkPoolNode.UpdateChunks();
		}

		public bool IsAutoTileHasAlpha( int autoTile_x, int autoTile_y )
		{
			if(IsValidAutoTilePos( autoTile_x, autoTile_y ))
			{
				return Tileset.IsAutoTileHasAlpha[ autoTile_y * AutoTileset.AutoTilesPerRow + autoTile_x ];
			}
			return false;
		}

		public bool IsAutoTileHasAlpha( int autoTileIdx )
		{
			if( (autoTileIdx >= 0) && (autoTileIdx < Tileset.IsAutoTileHasAlpha.Length) )
			{
				return Tileset.IsAutoTileHasAlpha[ autoTileIdx ];
			}
			return false;
		}

		public bool IsValidAutoTilePos( int autoTile_x, int autoTile_y )
		{
			return !(autoTile_x < 0 || autoTile_x >= m_AutoTileLayers[0].GetLength(0) || autoTile_y < 0 || autoTile_y >= m_AutoTileLayers[0].GetLength(1));
		}

		public AutoTile GetAutoTile( int autoTile_x, int autoTile_y, int iLayer )
		{
			if(IsValidAutoTilePos( autoTile_x, autoTile_y ))
			{
				return (m_AutoTileLayers == null || m_AutoTileLayers[iLayer][autoTile_x, autoTile_y] == null)? new AutoTile(){ Type = -1 } : m_AutoTileLayers[iLayer][autoTile_x, autoTile_y];			
			}
			return new AutoTile(){ Type = -1 };
		}

		// calculate tileset idx of autorile base in the number of tiles of each tileset
		private int _GetTilesetIdx( AutoTile _autoTile )
		{
			if( _autoTile.Type >= 0 )
			{
				int iCurTilesetIdx = 0;
				int tilemapCounter = 0;
				foreach( eTilesetType tilemapType in m_tilemapTypes )
				{
					tilemapCounter += _GetAutoTileNb( tilemapType );
					if( tilemapCounter > _autoTile.Type )
					{
						return iCurTilesetIdx;
					}
					++iCurTilesetIdx;
				}
			}
			return -1;
		}

		private int _GetAutoTileNb( eTilesetType _tilemapType )
		{
			int iRet;
			switch( _tilemapType )
			{
			case eTilesetType.ANIMATED: iRet = 16; break;
			case eTilesetType.GROUND:  iRet = 32; break;
			case eTilesetType.BUILDINGS:  iRet = 32; break;
			case eTilesetType.WALLS:  iRet = 48; break;
			case eTilesetType.NORMAL:  iRet = 128; break;
			case eTilesetType.OBJECTS:  iRet = 256; break;
			default: Debug.LogError("Undefined Type "+_tilemapType); iRet = 0; break;
			}
			return iRet;
		}

		public void SetAutoTile( int autoTile_x, int autoTile_y, int tile_type, int iLayer )
		{
			if( !IsValidAutoTilePos( autoTile_x, autoTile_y ) || iLayer >= m_AutoTileLayers.Count )
			{
				return;
			}

			tile_type = Mathf.Clamp( tile_type, -1, Tileset.ThumbnailSprites.Count-1 );

			AutoTile autoTile = m_AutoTileLayers[iLayer][autoTile_x, autoTile_y];
			if( autoTile == null)
			{
				autoTile = new AutoTile();
				m_AutoTileLayers[iLayer][autoTile_x, autoTile_y] = autoTile;
				autoTile.TilePartsType = new eTilePartType[4];
				autoTile.TilePartsIdx = new int[4];
			}
			autoTile.Type = tile_type;

			autoTile.AutoTileIdx = tile_type < 0? -1 : Tileset.AutotileIdxMap[tile_type];
			autoTile.TileX = autoTile_x;
			autoTile.TileY = autoTile_y;
			autoTile.Layer = iLayer;
			autoTile.TilesetIdx = _GetTilesetIdx( autoTile );

			// refresh tile and neighbours
			for( int xf = -1; xf < 2; ++xf  )
			{
				for( int yf = -1; yf < 2; ++yf  )
				{
					RefreshTile( autoTile_x+xf, autoTile_y+yf, iLayer );
				}
			}
		}

		private int[,] aTileAff = new int[,]
		{
			{2, 0},
			{0, 2},
			{2, 4},
			{2, 2},
			{0, 4},
		};
		
		private int[,] aTileBff = new int[,]
		{
			{3, 0},
			{3, 2},
			{1, 4},
			{1, 2},
			{3, 4},
		};
		
		private int[,] aTileCff = new int[,]
		{
			{2, 1},
			{0, 5},
			{2, 3},
			{2, 5},
			{0, 3},
		};
		
		private int[,] aTileDff = new int[,]
		{
			{3, 1},
			{3, 5},
			{1, 3},
			{1, 5},
			{3, 3},
		};

		public void RefreshTile( int autoTile_x, int autoTile_y, int iLayer )
		{
			AutoTile autoTile = GetAutoTile( autoTile_x, autoTile_y, iLayer );
			RefreshTile( autoTile );
		}

		public void RefreshTile( AutoTile autoTile )
		{
			m_tileChunkPoolNode.MarkUpdatedTile( autoTile.TileX, autoTile.TileY, autoTile.Layer);

			if( autoTile.Type >= 0 )
			{
				if( autoTile.Type >= 128 ) // 128 start with NORMAL tileset, treated differently )
				{
					int relativeTileIdx = autoTile.Type - 128; // relative idx to its tileset
					//check if its an object tileset
					if( autoTile.Type >= 256 )
					{
						relativeTileIdx -= 128;
						relativeTileIdx %= 256;
					}
					//
					int tx = relativeTileIdx  % AutoTileset.AutoTilesPerRow;
					int ty = relativeTileIdx / AutoTileset.AutoTilesPerRow;

					//fix tileset OBJECTS, the other part of the tileset in in the right side
					if( ty >= 16 )
					{
						ty -= 16;
						tx += 8;
					}
					//---

					int tileBaseIdx = Tileset.TilesetSpriteOffset[ autoTile.TilesetIdx ]; // set base tile idx of autoTile tileset
					int tileIdx = ( autoTile.Type >= 256 )? ty * 2 * AutoTileset.AutoTilesPerRow + tx : ty * AutoTileset.AutoTilesPerRow + tx;
					tileIdx +=  tileBaseIdx;

					autoTile.TilePartsIdx[ 0 ] = tileIdx;

					// set the kind of tile, for collision use
					autoTile.TilePartsType[ 0 ] = eTilePartType.EXT_CORNER;
					
					// Set Length of tileparts
					autoTile.TilePartsLength = 1;
				}
				else
				{
					int autoTile_x = autoTile.TileX;
					int autoTile_y = autoTile.TileY;
					int iLayer = autoTile.Layer;
					int tilePartIdx = 0;
					for( int j = 0; j < 2; ++j )
					{
						for( int i = 0; i < 2; ++i, ++tilePartIdx )
						{
							int tile_x = autoTile_x*2 + i;
							int tile_y = autoTile_y*2 + j;

							int tilePartX = 0;
							int tilePartY = 0;

							eTilePartType tilePartType;
							if (tile_x % 2 == 0 && tile_y % 2 == 0) //A
							{
								tilePartType = _getTileByNeighbours( autoTile_x, autoTile_y, autoTile.Type, 
								                               GetAutoTile( autoTile_x, autoTile_y-1, iLayer ).Type, //V 
								                               GetAutoTile( autoTile_x-1, autoTile_y, iLayer ).Type, //H 
								                               GetAutoTile( autoTile_x-1, autoTile_y-1, iLayer ).Type  //D
								                               );
								tilePartX = aTileAff[ (int)tilePartType, 0 ];
								tilePartY = aTileAff[ (int)tilePartType, 1 ];
							} 
							else if (tile_x % 2 != 0 && tile_y % 2 == 0) //B
							{
								tilePartType = _getTileByNeighbours( autoTile_x, autoTile_y, autoTile.Type, 
								                               GetAutoTile( autoTile_x, autoTile_y-1, iLayer ).Type, //V 
								                               GetAutoTile( autoTile_x+1, autoTile_y, iLayer ).Type, //H 
								                               GetAutoTile( autoTile_x+1, autoTile_y-1, iLayer ).Type  //D
								                               );
								tilePartX = aTileBff[ (int)tilePartType, 0 ];
								tilePartY = aTileBff[ (int)tilePartType, 1 ];
							}
							else if (tile_x % 2 == 0 && tile_y % 2 != 0) //C
							{
								tilePartType = _getTileByNeighbours( autoTile_x, autoTile_y, autoTile.Type, 
								                               GetAutoTile( autoTile_x, autoTile_y+1, iLayer ).Type, //V 
								                               GetAutoTile( autoTile_x-1, autoTile_y, iLayer ).Type, //H 
								                               GetAutoTile( autoTile_x-1, autoTile_y+1, iLayer ).Type  //D
								                               );
								tilePartX = aTileCff[ (int)tilePartType, 0 ];
								tilePartY = aTileCff[ (int)tilePartType, 1 ];
							}
							else //if (tile_x % 2 != 0 && tile_y % 2 != 0) //D
							{
								tilePartType = _getTileByNeighbours( autoTile_x, autoTile_y, autoTile.Type, 
								                               GetAutoTile( autoTile_x, autoTile_y+1, iLayer ).Type, //V 
								                               GetAutoTile( autoTile_x+1, autoTile_y, iLayer ).Type, //H 
								                               GetAutoTile( autoTile_x+1, autoTile_y+1, iLayer ).Type  //D
								                               );
								tilePartX = aTileDff[ (int)tilePartType, 0 ];
								tilePartY = aTileDff[ (int)tilePartType, 1 ];
							}

							// set the kind of tile, for collision use
							autoTile.TilePartsType[ tilePartIdx ] = tilePartType;

							int tileBaseIdx = Tileset.TilesetSpriteOffset[ autoTile.TilesetIdx ]; // set base tile idx of autoTile tileset
							//NOTE: All tileset have 32 autotiles except the Wall tileset with 48 tiles ( so far it's working because wall tileset is the last one )
							int relativeTileIdx = autoTile.AutoTileIdx - (autoTile.TilesetIdx * 32); // relative to owner tileset ( All tileset have 32 autotiles )
							int tx = relativeTileIdx  % AutoTileset.AutoTilesPerRow;
							int ty = relativeTileIdx / AutoTileset.AutoTilesPerRow;
							int tilePartSpriteIdx;
							if( m_tilemapTypes[autoTile.TilesetIdx] == eTilesetType.BUILDINGS )
							{
								tilePartY = Mathf.Max( 0, tilePartY - 2);
								tilePartSpriteIdx = tileBaseIdx + ty * (AutoTileset.AutoTilesPerRow * 4) * 4 + tx * 4 + tilePartY * (AutoTileset.AutoTilesPerRow * 4) + tilePartX;
							}
							//NOTE: It's not working with stairs shapes
							// XXXXXX
							// IIIXXX
							// IIIXXX
							// IIIIII
							else if( m_tilemapTypes[autoTile.TilesetIdx] == eTilesetType.WALLS )
							{
								if( ty % 2 == 0 )
								{
									tilePartSpriteIdx = tileBaseIdx + (ty/2) * (AutoTileset.AutoTilesPerRow * 4) * 10 + tx * 4 + tilePartY * (AutoTileset.AutoTilesPerRow * 4) + tilePartX;
								}
								else
								{
									//tilePartY = Mathf.Max( 0, tilePartY - 2);
									tilePartY -= 2;
									if( tilePartY < 0 )
									{
										if( tilePartX == 2 && tilePartY == -2 ) 	 {tilePartX = 2; tilePartY = 0;}
										else if( tilePartX == 3 && tilePartY == -2 ) {tilePartX = 1; tilePartY = 0;}
										else if( tilePartX == 2 && tilePartY == -1 ) {tilePartX = 2; tilePartY = 3;}
										else if( tilePartX == 3 && tilePartY == -1 ) {tilePartX = 1; tilePartY = 3;}
									}
									tilePartSpriteIdx = tileBaseIdx + (AutoTileset.AutoTilesPerRow * 4) * ((ty/2) * 10 + 6) + tx * 4 + tilePartY * (AutoTileset.AutoTilesPerRow * 4) + tilePartX;
								}
							}
							else
							{
								tilePartSpriteIdx = tileBaseIdx + ty * (AutoTileset.AutoTilesPerRow * 4) * 6 + tx * 4 + tilePartY * (AutoTileset.AutoTilesPerRow * 4) + tilePartX;
							}

							autoTile.TilePartsIdx[ tilePartIdx ] = tilePartSpriteIdx;

							// Set Length of tileparts
							autoTile.TilePartsLength = 4;
						}
					}
				}
			}
		}

		public eTileCollisionType GetAutotileCollisionAtPosition( Vector3 vPos )
		{
			vPos -= transform.position;

			// transform to pixel coords
			vPos.y = -vPos.y;

			vPos *= AutoTileset.PixelToUnits;
			if( vPos.x >= 0 && vPos.y >= 0 )
			{
				int tile_x = (int)vPos.x / AutoTileset.TileWidth;
				int tile_y = (int)vPos.y / AutoTileset.TileWidth;
				Vector2 vTileOffset = new Vector2( (int)vPos.x % AutoTileset.TileWidth, (int)vPos.y % AutoTileset.TileHeight );
				for( int iLayer = (int)eTileLayer._SIZE - 1; iLayer >= 0; --iLayer )
				{
					eTileCollisionType tileCollType = GetAutotileCollision( tile_x, tile_y, iLayer, vTileOffset );
					if( tileCollType != eTileCollisionType.EMPTY && tileCollType != eTileCollisionType.OVERLAY )
					{
						return tileCollType;
					}
				}
			}
			return eTileCollisionType.PASSABLE;
		}

		public eTileCollisionType GetAutotileCollision( int tile_x, int tile_y, int layer, Vector2 vTileOffset )
		{
			if( IsCollisionEnabled )
			{
				AutoTile autoTile = GetAutoTile( tile_x, tile_y, layer );
				if( autoTile != null && autoTile.Type >= 0 && autoTile.TilePartsIdx != null )
				{
					Vector2 vTilePartOffset = new Vector2( vTileOffset.x % AutoTileset.TilePartWidth, vTileOffset.y % AutoTileset.TilePartHeight );
					int tilePartIdx = autoTile.TilePartsLength == 4? 2*((int)vTileOffset.y / AutoTileset.TilePartHeight) + ((int)vTileOffset.x / AutoTileset.TilePartWidth) : 0;
					eTileCollisionType tileCollType = _GetTilePartCollision( Tileset.AutotileCollType[ autoTile.Type ], autoTile.TilePartsType[tilePartIdx], tilePartIdx, vTilePartOffset );
					return tileCollType;
				}
			}
			return eTileCollisionType.EMPTY;
		}

		// NOTE: depending of the collType and tilePartType, this method returns the collType or eTileCollisionType.PASSABLE
		// This is for special tiles like Fence and Wall where not all of tile part should return collisions
		eTileCollisionType _GetTilePartCollision( eTileCollisionType collType, eTilePartType tilePartType, int tilePartIdx, Vector2 vTilePartOffset )
		{
			int tilePartHalfW = AutoTileset.TilePartWidth/2;
			int tilePartHalfH = AutoTileset.TilePartHeight/2;
			if( collType == eTileCollisionType.FENCE )
			{
				if( tilePartType == eTilePartType.EXT_CORNER || tilePartType == eTilePartType.V_SIDE )
				{
					// now check inner collision ( half left for tile AC and half right for tiles BD )
					// AX|BX|A1|B1	A: 0
					// AX|BX|C1|D1	B: 1
					// A2|B4|A4|B2	C: 2
					// C5|D3|C3|D5	D: 3
					// A5|B3|A3|B5
					// C2|D4|C4|D2
					if( 
					   (tilePartIdx == 0 || tilePartIdx == 2) && (vTilePartOffset.x < tilePartHalfW ) ||
					   (tilePartIdx == 1 || tilePartIdx == 3) && (vTilePartOffset.x > tilePartHalfW )
					)
					{
						return eTileCollisionType.PASSABLE;
					}
				}
			}
			else if( collType == eTileCollisionType.WALL )
			{
				if( tilePartType == eTilePartType.INTERIOR )
				{
					return eTileCollisionType.PASSABLE;
				}
				else if( tilePartType == eTilePartType.H_SIDE )
				{
					if( 
					   (tilePartIdx == 0 || tilePartIdx == 1) && (vTilePartOffset.y >= tilePartHalfH ) ||
					   (tilePartIdx == 2 || tilePartIdx == 3) && (vTilePartOffset.y < tilePartHalfH )
					   )
					{
						return eTileCollisionType.PASSABLE;
					}
				}
				else if( tilePartType == eTilePartType.V_SIDE )
				{
					if( 
					   (tilePartIdx == 0 || tilePartIdx == 2) && (vTilePartOffset.x >= tilePartHalfW ) ||
					   (tilePartIdx == 1 || tilePartIdx == 3) && (vTilePartOffset.x < tilePartHalfW )
					   )
					{
						return eTileCollisionType.PASSABLE;
					}
				}
				else
				{
					Vector2 vRelToIdx0 = vTilePartOffset; // to check only the case (tilePartIdx == 0) vTilePartOffset coords are mirrowed to put position over tileA with idx 0
					vRelToIdx0.x = (int)vRelToIdx0.x; // avoid precission errors when mirrowing, as 0.2 is 0, but -0.2 is 0 as well and should be -1
					vRelToIdx0.y = (int)vRelToIdx0.y;
					if( tilePartIdx == 1 ) vRelToIdx0.x = -vRelToIdx0.x + AutoTileset.TilePartWidth - 1;
					else if( tilePartIdx == 2 ) vRelToIdx0.y = -vRelToIdx0.y + AutoTileset.TilePartHeight - 1;
					else if( tilePartIdx == 3 ) vRelToIdx0 = -vRelToIdx0 + new Vector2( AutoTileset.TilePartWidth - 1, AutoTileset.TilePartHeight - 1 );

					if( tilePartType == eTilePartType.INT_CORNER )
					{
						if( (int)vRelToIdx0.x / tilePartHalfW == 1 || (int)vRelToIdx0.y / tilePartHalfH == 1 )
						{
							return eTileCollisionType.PASSABLE;
						}
					}
					else if( tilePartType == eTilePartType.EXT_CORNER )
					{
						if( (int)vRelToIdx0.x / tilePartHalfW == 1 && (int)vRelToIdx0.y / tilePartHalfH == 1 )
						{
							return eTileCollisionType.PASSABLE;
						}
					}

				}
			}
			return collType;
		}

		// V vertical, H horizontal, D diagonal
		private eTilePartType _getTileByNeighbours( int autoTile_x, int autoTile_y, int tile_type, int tile_typeV, int tile_typeH, int tile_typeD )
		{
			if (
				(tile_typeV == tile_type) &&
				(tile_typeH == tile_type) &&
				(tile_typeD != tile_type)
				) 
			{
				return eTilePartType.INT_CORNER;
			}
			else if (
				(tile_typeV != tile_type) &&
				(tile_typeH != tile_type)
				) 
			{
				return eTilePartType.EXT_CORNER;
			}
			else if (
				(tile_typeV == tile_type) &&
				(tile_typeH == tile_type) &&
				(tile_typeD == tile_type)
				) 
			{
				return eTilePartType.INTERIOR;
			}
			else if (
				(tile_typeV != tile_type) &&
				(tile_typeH == tile_type)
				) 
			{
				return eTilePartType.H_SIDE;
			}
			else /*if (
				(tile_typeV == tile_type) &&
				(tile_typeH != tile_type)
				)*/
			{
				return eTilePartType.V_SIDE;
			}
		}

		Color _support_GetAvgColorOfTexture( Texture2D _texture, Rect _srcRect )
		{
			float r, g, b, a;
			r = g = b = a = 0;
			Color[] aColors = _texture.GetPixels( Mathf.RoundToInt(_srcRect.x), Mathf.RoundToInt(_srcRect.y), Mathf.RoundToInt(_srcRect.width), Mathf.RoundToInt(_srcRect.height));
			for( int i = 0; i < aColors.Length; ++i )
			{
				r += aColors[i].r;
				g += aColors[i].g;
				b += aColors[i].b;
				a += aColors[i].a;
			}
			r /= aColors.Length;
			g /= aColors.Length;
			b /= aColors.Length;
			a /= aColors.Length;
			return new Color(r, g, b, a);
		}

		void _GenerateMinimapTilesTexture()
		{
			Color[] aColors = Enumerable.Repeat<Color>( new Color(0f, 0f, 0f, 0f) , m_minimapTilesTexture.GetPixels().Length).ToArray();

			Rect srcRect = new Rect( 0, 0, AutoTileset.TileWidth, AutoTileset.TileHeight );
			int idx = 0;
			foreach( eTilesetGroupType groupType in Enum.GetValues(typeof(eTilesetGroupType )))
			{
				Texture2D thumbTex = UtilsAutoTileMap.GenerateTilesetTexture( Tileset.TilesetsAtlasTexture, groupType);
				for( srcRect.y = thumbTex.height - AutoTileset.TileHeight; srcRect.y >= 0; srcRect.y -= AutoTileset.TileHeight )
				{
					for( srcRect.x = 0; srcRect.x < thumbTex.width; srcRect.x += AutoTileset.TileWidth, ++idx )
					{
						// improved tile color by using the center square as some autotiles are surrounded by ground pixels like water tiles
						Rect rRect = new Rect( srcRect.x + srcRect.width/4, srcRect.y + srcRect.height/4, srcRect.width/2, srcRect.height/2 );
						aColors[idx] = _support_GetAvgColorOfTexture( thumbTex, rRect );
					}
				}
			}
			
			m_minimapTilesTexture.SetPixels( aColors );
			m_minimapTilesTexture.Apply();
		}

		public void RefreshMinimapTexture( )
		{
			RefreshMinimapTexture( 0, 0, MapTileWidth, MapTileHeight );
		}

		public void RefreshMinimapTexture( int tile_x, int tile_y, int width, int height )
		{
			tile_x = Mathf.Clamp( tile_x, 0, MinimapTexture.width - 1 );
			tile_y = Mathf.Clamp( tile_y, 0, MinimapTexture.height - 1 );
			width = Mathf.Min( width, MinimapTexture.width - tile_x );
			height = Mathf.Min( height, MinimapTexture.height - tile_y );

			Color[] aTilesColors = m_minimapTilesTexture.GetPixels();
			Color[] aMinimapColors = Enumerable.Repeat<Color>( new Color(0f, 0f, 0f, 1f) , MinimapTexture.GetPixels(tile_x, MinimapTexture.height - tile_y - height, width, height).Length).ToArray();
			foreach( AutoTile[,] aAutoTiles in m_AutoTileLayers )
			{
				// read tile type in the same way that texture pixel order, from bottom to top, right to left
				for( int yf = 0; yf < height; ++yf )
				{
					for( int xf = 0; xf < width; ++xf )
					{
						int tx = tile_x + xf;
						int ty = tile_y + yf;

						int type = aAutoTiles[ tx, ty] != null? aAutoTiles[tx, ty].Type : -1;
						if( type >= 0 )
						{
							int idx = (height-1-yf)*width + xf;
							Color baseColor = aMinimapColors[idx];
							Color tileColor = aTilesColors[type];
							aMinimapColors[idx] = baseColor*(1-tileColor.a) + tileColor*tileColor.a ;
							aMinimapColors[idx].a = 1f;
						}
					}
				}
			}
			MinimapTexture.SetPixels( tile_x, MinimapTexture.height - tile_y - height, width, height, aMinimapColors );
			MinimapTexture.Apply();
		}
	}
}
