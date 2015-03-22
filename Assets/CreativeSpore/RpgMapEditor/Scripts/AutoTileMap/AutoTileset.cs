using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreativeSpore.RpgMapEditor
{
	public class AutoTileset : ScriptableObject 
	{
		public const int TileWidth = 32;
		public const int TileHeight = 32;
		public const int TilePartWidth = 16;
		public const int TilePartHeight = 16;
		public const int AutoTilesPerRow = 8;
		public const float PixelToUnits = 100f;
		
		public const float TileWorldWidth = TileWidth / PixelToUnits;
		public const float TileWorldHeight = TileHeight / PixelToUnits;

		public Texture2D AnimatedTexture;
		public Texture2D GroundTexture;
		public Texture2D BuildingTexture;
		public Texture2D WallTexture;
		public Texture2D NormalTexture;
		public Texture2D ObjectsTexture_B;
		public Texture2D ObjectsTexture_C;
		public Texture2D ObjectsTexture_D;
		public Texture2D ObjectsTexture_E;

		public AutoTileMap.eTileCollisionType[] AutotileCollType = new AutoTileMap.eTileCollisionType[1280]; //1280 the total number of autotiles	

		public bool IsAutoTileAnimated( int autoTileIdx )
		{
			return (autoTileIdx >= 0 && autoTileIdx < 16 && (autoTileIdx%2) == 0  );
		}

        public bool IsAutoTileAnimatedWaterfall(int autoTileIdx)
        {
            return (autoTileIdx >= 0 && autoTileIdx < 16 && (autoTileIdx % 2) != 0 && autoTileIdx != 1 && autoTileIdx != 5 );
        }

		public bool[] IsAutoTileHasAlpha;
		public List<Sprite> ThumbnailSprites;
		public List<Sprite> AutoTileSprites;
		public int[] AutotileIdxMap; // map tileIdx with tilesetTileIdx

		public int[] TilesetSpriteOffset; // for each tileset in TilemapTextures the index of the first tile part in m_TilemapSprites

		public Material AtlasMaterial{ get; private set; }

		[SerializeField]
		private Texture2D m_atlasTexture;
		public Texture2D TilesetsAtlasTexture
		{  
			get{return m_atlasTexture;}
			set
			{
				if( value != null && value != m_atlasTexture )
				{
					if( value.width == 2048 && value.height == 2048 )
					{
						m_atlasTexture = value;
						UtilsAutoTileMap.ImportTexture( m_atlasTexture );
						GenerateAutoTileData();

						if( AtlasMaterial == null )
						{
							CreateAtlasMaterial();
						}
					}
					else
					{
						m_atlasTexture = null;
						Debug.LogError( " TilesetsAtlasTexture.set: atlas texture is not 2048x2048" );
					}
				}
				else
				{
					m_atlasTexture = value;
				}
			}
		}

		public void GenerateAutoTileData( )
		{

			// force to generate atlas material if it is not instanciated
			if( AtlasMaterial == null )
			{
				//Debug.LogError( "GenerateAutoTileData error: missing AtlasMaterial" );
				//return;
				CreateAtlasMaterial();
			}

			_GenerateTilesetSprites();

			// get the mapped tileIdx ( for animated tile supporting. Animated tiles are considered as one, skipping the other 2 frames )
			AutotileIdxMap = new int[1280];
			int tileIdx = 0;
			for( int i = 0; i < 1280; ++i )
			{
				AutotileIdxMap[i] = tileIdx;
				tileIdx += IsAutoTileAnimated(i)? 3 : 1;
			}

			IsAutoTileHasAlpha = new bool[1280];
			ThumbnailSprites = new List<Sprite>(1280);
			UtilsAutoTileMap.FillWIthTilesetThumbnailSprites(ThumbnailSprites, TilesetsAtlasTexture, AutoTileMap.eTilesetGroupType.GROUND);
			UtilsAutoTileMap.FillWIthTilesetThumbnailSprites(ThumbnailSprites, TilesetsAtlasTexture, AutoTileMap.eTilesetGroupType.OBJECTS_B);
			UtilsAutoTileMap.FillWIthTilesetThumbnailSprites(ThumbnailSprites, TilesetsAtlasTexture, AutoTileMap.eTilesetGroupType.OBJECTS_C);
			UtilsAutoTileMap.FillWIthTilesetThumbnailSprites(ThumbnailSprites, TilesetsAtlasTexture, AutoTileMap.eTilesetGroupType.OBJECTS_D);
			UtilsAutoTileMap.FillWIthTilesetThumbnailSprites(ThumbnailSprites, TilesetsAtlasTexture, AutoTileMap.eTilesetGroupType.OBJECTS_E);

			//+++ sometimes png texture loose isReadable value. Maybe a unity bug?
	#if UNITY_EDITOR
			string assetPath = AssetDatabase.GetAssetPath(TilesetsAtlasTexture);
			TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter; 
			if( textureImporter != null && textureImporter.isReadable == false )
			{	// reimport texture
                Debug.LogWarning("TilesetsAtlasTexture " + assetPath + " isReadable is false. Will be re-imported to access pixels.");
				UtilsAutoTileMap.ImportTexture( TilesetsAtlasTexture );
			}
	#endif
			//---
			Color32[] aAtlasColors = TilesetsAtlasTexture.GetPixels32(); //NOTE: Color32 is faster than Color in this alpha check
			for( int i = 0; i < ThumbnailSprites.Count; ++i )
			{
				if( i >= 48 && i < 128 )
				{
					// wall and building tiles has no alpha by default
					IsAutoTileHasAlpha[i] = false;
				}
				else
				{
					Sprite sprTile = ThumbnailSprites[i];
                    int pBaseIdx = (int)(sprTile.rect.y * sprTile.texture.width + sprTile.rect.x);
					for( float py = 0; py < AutoTileset.TileHeight && !IsAutoTileHasAlpha[ i ]; py++ )
					{
						for( float px = 0; px < AutoTileset.TileWidth && !IsAutoTileHasAlpha[ i ]; px++ )
						{
							int pIdx = pBaseIdx + (int)(py * sprTile.texture.width + px);
							if( aAtlasColors[pIdx].a < 255 )
							{
								IsAutoTileHasAlpha[ i ] = true;
							}
						}
					}
				}
			}
	#if UNITY_EDITOR
			EditorUtility.SetDirty( this );
	#endif

		}

		private void CreateAtlasMaterial()
		{
			string matPath = "";
#if UNITY_EDITOR
			matPath = System.IO.Path.GetDirectoryName( AssetDatabase.GetAssetPath( m_atlasTexture ) );
			if( !string.IsNullOrEmpty( matPath ) )
			{
				matPath += "/"+TilesetsAtlasTexture.name+" atlas material.mat";
				Material matAtlas = (Material)AssetDatabase.LoadAssetAtPath( matPath, typeof(Material));
				if( matAtlas == null )
				{
					matAtlas = new Material( Shader.Find("Unlit/Transparent") ); //NOTE: if this material changes, remember to change also the one inside #else #endif below
					AssetDatabase.CreateAsset(matAtlas, matPath );
				}
				AtlasMaterial = matAtlas;
				EditorUtility.SetDirty( AtlasMaterial );
				AssetDatabase.SaveAssets();
            }
#else
			AtlasMaterial = new Material( Shader.Find("Unlit/Transparent") );
#endif

            if ( AtlasMaterial != null )
			{
				AtlasMaterial.mainTexture = TilesetsAtlasTexture;
			}
			else
			{
				m_atlasTexture = null;
				Debug.LogError( " TilesetsAtlasTexture.set: there was an error creating the material asset at "+matPath );
			}
		}

		private void _support_generateTileparts( Texture2D tilesetTex, string defaultName, int srcX, int srcY, int width, int height, int tileWidth, int tileHeight )
		{
			Vector2 pivot = new Vector2(0f,1f);
			int iTilesetSprIdx = 0;
			Rect rec = new Rect( 0, 0, tileWidth, tileHeight );
            //+++fast fix> due a Unity Pro bug, Sprite.Create is very slow for values equal or above 32
            //TODO: check when this is fixed to remove this code
            if (tileWidth == 32) rec.width = 31.9f;
            if (tileHeight == 32) rec.height = 31.9f;
            //---
            string tilesetName = tilesetTex != null ? tilesetTex.name : defaultName;
			for( int y = height - tileHeight; y >= 0; y -= tileHeight )
			{
				for( int x = 0; x < width; x+=tileWidth, ++iTilesetSprIdx )
				{
					rec.x = srcX + x;
					rec.y = srcY + y;
					
					Sprite tileSpr = Sprite.Create(TilesetsAtlasTexture, rec, pivot, PixelToUnits);

					tileSpr.name = tilesetName+"_"+iTilesetSprIdx;
					AutoTileSprites.Add( tileSpr );
				}
			}
		}
		
		private void _GenerateTilesetSprites()
		{
			TilesetSpriteOffset = new int[9];
			AutoTileSprites = new List<Sprite>(4160);
			AutoTileSprites.Clear();
			
			TilesetSpriteOffset[0] = AutoTileSprites.Count;
			_support_generateTileparts(AnimatedTexture, "Animated_A1", 0, 0, 512, 384, TilePartWidth, TilePartHeight);		//Animated
			TilesetSpriteOffset[1] = AutoTileSprites.Count;
			_support_generateTileparts(GroundTexture, "Ground_A2", 0, 384, 512, 384, TilePartWidth, TilePartHeight);	//Ground
			TilesetSpriteOffset[2] = AutoTileSprites.Count;
			_support_generateTileparts(BuildingTexture, "Building_A3", 0, 768, 512, 256, TilePartWidth, TilePartHeight);	//Building
			TilesetSpriteOffset[3] = AutoTileSprites.Count;
			_support_generateTileparts(WallTexture, "Wall_A4", 512, 0, 512, 480, TilePartWidth, TilePartHeight);	//Walls
			TilesetSpriteOffset[4] = AutoTileSprites.Count;
			_support_generateTileparts(NormalTexture, "Normal_A5", 512, 512, 256, 512, TileWidth, TileHeight);			//Normal
			TilesetSpriteOffset[5] = AutoTileSprites.Count;
			
			_support_generateTileparts(ObjectsTexture_B, "Objects_B", 1024, 0, 512, 512, TileWidth, TileHeight);		// Objects B
			TilesetSpriteOffset[6] = AutoTileSprites.Count;
			_support_generateTileparts(ObjectsTexture_C, "Objects_C", 1024, 512, 512, 512, TileWidth, TileHeight);		// Objects C
			TilesetSpriteOffset[7] = AutoTileSprites.Count;
			_support_generateTileparts(ObjectsTexture_D, "Objects_D", 1536, 0, 512, 512, TileWidth, TileHeight);		// Objects D
			TilesetSpriteOffset[8] = AutoTileSprites.Count;
			_support_generateTileparts(ObjectsTexture_E, "Objects_E", 1536, 512, 512, 512, TileWidth, TileHeight);		// Objects E
		}
		
	}
}