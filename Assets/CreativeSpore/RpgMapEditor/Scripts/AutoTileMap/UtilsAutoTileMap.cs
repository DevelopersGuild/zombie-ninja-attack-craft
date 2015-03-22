using UnityEngine;
using System.Collections;
using System.Linq;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreativeSpore.RpgMapEditor
{
	public class UtilsAutoTileMap 
	{
		public static Texture2D GenerateTilesetTexture( Texture2D _atlasTexture, AutoTileMap.eTilesetGroupType _tilesetGroupType )
		{
			List<Sprite> sprList = new List<Sprite>();
			FillWIthTilesetThumbnailSprites( sprList, _atlasTexture, _tilesetGroupType);
			Texture2D tilesetTexture = new Texture2D( 256, 1024 );

			int sprIdx = 0;
			Rect dstRect = new Rect( 0, tilesetTexture.height - AutoTileset.TileHeight, AutoTileset.TileWidth, AutoTileset.TileHeight);
			for( ; dstRect.y >= 0; dstRect.y -= AutoTileset.TileHeight )
			{
				for( dstRect.x = 0; dstRect.x < tilesetTexture.width && sprIdx < sprList.Count; dstRect.x += AutoTileset.TileWidth, ++sprIdx )
				{
					Rect srcRect = sprList[sprIdx].rect;
                    //TODO: this code is for testing purposes
                    #if UNITY_EDITOR
                    if (Mathf.RoundToInt(srcRect.x) > 2016 || Mathf.RoundToInt(srcRect.y) > 2016 || Mathf.RoundToInt(srcRect.x) < 0 || Mathf.RoundToInt(srcRect.y) < 0 )
                    {
                        string sError = " Weird Error: group: " + _tilesetGroupType + " sprIdx " + sprIdx + " textureRect " + sprList[sprIdx].textureRect + " rect " + sprList[sprIdx].rect;
                        Debug.LogError( sError );
                        //EditorUtility.DisplayDialog("Weird bug", sError, "Ok");
                        continue;
                    }
                    #endif
                    //---                    
					Color[] autotileColors = sprList[sprIdx].texture.GetPixels( Mathf.RoundToInt(srcRect.x), Mathf.RoundToInt(srcRect.y), AutoTileset.TileWidth, AutoTileset.TileHeight );
					tilesetTexture.SetPixels( Mathf.RoundToInt(dstRect.x), Mathf.RoundToInt(dstRect.y), AutoTileset.TileWidth, AutoTileset.TileHeight, autotileColors);
				}
			}
			tilesetTexture.Apply();

			return tilesetTexture;
		}

		public static void FillWIthTilesetThumbnailSprites( List<Sprite> _outList, Texture2D _atlasTexture, AutoTileMap.eTilesetGroupType _tilesetGroupType )
		{
			Vector2 pivot = new Vector2(0f,1f);
			Rect sprRect = new Rect(0, 0, AutoTileset.TileWidth, AutoTileset.TileHeight);
            //+++fast fix> due a Unity Pro bug, Sprite.Create is very slow for values equal or above 32
            //TODO: check when this is fixed to remove this code
            sprRect.width = 31.9f;
            sprRect.height = 31.9f;
            //---
			if( _tilesetGroupType == AutoTileMap.eTilesetGroupType.GROUND )
			{
				// animated
				for( sprRect.y = 384-AutoTileset.TileHeight; sprRect.y >= 0; sprRect.y -= 3*AutoTileset.TileHeight )
				{
					int tx;
					for( tx = 0, sprRect.x = 0; sprRect.x < 512; sprRect.x += 2*AutoTileset.TileWidth, ++tx )
					{
						if( tx % 4 == 0 || tx % 4 == 3 )
						{
                            Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
							_outList.Add(sprAutoTile);
						}
					}
				}

				// ground
				for( sprRect.y = 768-AutoTileset.TileHeight; sprRect.y >= 384; sprRect.y -= 3*AutoTileset.TileHeight )
				{
					for( sprRect.x = 0; sprRect.x < 512; sprRect.x += 2*AutoTileset.TileWidth )
					{
                        Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
						_outList.Add(sprAutoTile);
					}
				}

				// building
				for( sprRect.y = 512+3*AutoTileset.TileHeight; sprRect.y >= 512; sprRect.y -= AutoTileset.TileHeight )
				{
					for( sprRect.x = 768; sprRect.x < 768+8*AutoTileset.TileWidth; sprRect.x += AutoTileset.TileWidth )
					{
                        Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
						_outList.Add(sprAutoTile);
					}
				}

				// walls
				sprRect.y = (15-1)*AutoTileset.TileHeight;
				for( sprRect.x = 512; sprRect.x < 1024; sprRect.x += 2*AutoTileset.TileWidth )
				{
                    Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
					_outList.Add(sprAutoTile);
				}
				sprRect.y = 640 + 2*AutoTileset.TileHeight;
				for( sprRect.x = 768; sprRect.x < 768+8*AutoTileset.TileWidth; sprRect.x += AutoTileset.TileWidth )
				{
                    Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
					_outList.Add(sprAutoTile);
				}
				sprRect.y = (10-1)*AutoTileset.TileHeight;
				for( sprRect.x = 512; sprRect.x < 1024; sprRect.x += 2*AutoTileset.TileWidth )
				{
                    Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
					_outList.Add(sprAutoTile);
				}
				sprRect.y = 640 + AutoTileset.TileHeight;
				for( sprRect.x = 768; sprRect.x < 768+8*AutoTileset.TileWidth; sprRect.x += AutoTileset.TileWidth )
				{
                    Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
					_outList.Add(sprAutoTile);
				}
				sprRect.y = (5-1)*AutoTileset.TileHeight;
				for( sprRect.x = 512; sprRect.x < 1024; sprRect.x += 2*AutoTileset.TileWidth )
				{
                    Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
					_outList.Add(sprAutoTile);
				}
				sprRect.y = 640;
				for( sprRect.x = 768; sprRect.x < 768+8*AutoTileset.TileWidth; sprRect.x += AutoTileset.TileWidth )
				{
                    Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
					_outList.Add(sprAutoTile);
				}
				//--- walls

				//Normal
				_FillSpritesFromRect(_outList, _atlasTexture, 512, 512, 256, 512);
			}
			else if( _tilesetGroupType == AutoTileMap.eTilesetGroupType.OBJECTS_B )
			{
				_FillSpritesFromRect(_outList, _atlasTexture, 1024, 0, 256, 512);
				_FillSpritesFromRect(_outList, _atlasTexture, 1280, 0, 256, 512);
			}
			else if( _tilesetGroupType == AutoTileMap.eTilesetGroupType.OBJECTS_C )
			{
				_FillSpritesFromRect(_outList, _atlasTexture, 1024, 512, 256, 512);
				_FillSpritesFromRect(_outList, _atlasTexture, 1280, 512, 256, 512);
			}
			else if( _tilesetGroupType == AutoTileMap.eTilesetGroupType.OBJECTS_D )
			{
				_FillSpritesFromRect(_outList, _atlasTexture, 1536, 0, 256, 512);
				_FillSpritesFromRect(_outList, _atlasTexture, 1792, 0, 256, 512);
			}
			else if( _tilesetGroupType == AutoTileMap.eTilesetGroupType.OBJECTS_E )
			{
				_FillSpritesFromRect(_outList, _atlasTexture, 1536, 512, 256, 512);
				_FillSpritesFromRect(_outList, _atlasTexture, 1792, 512, 256, 512);
			}
		}

		private static void _FillSpritesFromRect( List<Sprite> _outList, Texture2D _atlasTexture, int x, int y, int width, int height )
		{
			Vector2 pivot = new Vector2(0f,1f);
			Rect srcRect = new Rect(0, 0, AutoTileset.TileWidth, AutoTileset.TileHeight);
            //+++fast fix> due a Unity Pro bug, Sprite.Create is very slow for values equal or above 32
            //TODO: check when this is fixed to remove this code
            srcRect.width = 31.9f;
            srcRect.height = 31.9f;
            //---
			for( srcRect.y = height - AutoTileset.TileHeight; srcRect.y >= 0; srcRect.y -= AutoTileset.TileHeight )
			{
				for( srcRect.x = 0; srcRect.x < width; srcRect.x += AutoTileset.TileWidth )
				{
					Rect sprRect = srcRect;
					sprRect.x += x;
					sprRect.y += y;
                    Sprite sprAutoTile = Sprite.Create(_atlasTexture, sprRect, pivot, AutoTileset.PixelToUnits);
					_outList.Add(sprAutoTile);
				}
			}
		}

		public static Texture2D GenerateAtlas( AutoTileset _autoTileset )
		{
			ImportTexture( _autoTileset.AnimatedTexture );
			ImportTexture( _autoTileset.GroundTexture );
			ImportTexture( _autoTileset.BuildingTexture );
			ImportTexture( _autoTileset.WallTexture );
			ImportTexture( _autoTileset.NormalTexture ); 
			ImportTexture( _autoTileset.ObjectsTexture_B ); 
			ImportTexture( _autoTileset.ObjectsTexture_C ); 
			ImportTexture( _autoTileset.ObjectsTexture_D ); 
			ImportTexture( _autoTileset.ObjectsTexture_E ); 
			
			Texture2D atlasTexture = new Texture2D(2048, 2048);
			Color32[] atlasColors = Enumerable.Repeat<Color32>( new Color32(0, 0, 0, 0) , 2048*2048).ToArray();
			atlasTexture.SetPixels32(atlasColors);
			_CopyTilesetInAtlas(atlasTexture, _autoTileset.AnimatedTexture, 0, 0, 512, 384);
			_CopyTilesetInAtlas(atlasTexture, _autoTileset.GroundTexture, 0, 384, 512, 384);
			_CopyTilesetInAtlas(atlasTexture, _autoTileset.BuildingTexture, 0, 768, 512, 256);
			_CopyTilesetInAtlas(atlasTexture, _autoTileset.WallTexture, 512, 0, 512, 480);
			_CopyTilesetInAtlas(atlasTexture, _autoTileset.NormalTexture, 512, 512, 256, 512);
			_CopyTilesetInAtlas(atlasTexture, _autoTileset.ObjectsTexture_B, 1024, 0, 512, 512);
			_CopyTilesetInAtlas(atlasTexture, _autoTileset.ObjectsTexture_C, 1024, 512, 512, 512);
			_CopyTilesetInAtlas(atlasTexture, _autoTileset.ObjectsTexture_D, 1536, 0, 512, 512);
			_CopyTilesetInAtlas(atlasTexture, _autoTileset.ObjectsTexture_E, 1536, 512, 512, 512);
			_CopyBuildingThumbnails(atlasTexture,  _autoTileset.BuildingTexture, 768, 512 );
			_CopyWallThumbnails(atlasTexture,  _autoTileset.WallTexture, 768, 640 );
			atlasTexture.Apply();

			return atlasTexture;
		}


		public static bool ImportTexture( Texture2D texture )
		{
	#if UNITY_EDITOR
			if( texture != null )
			{
				return ImportTexture( AssetDatabase.GetAssetPath(texture) );
			}
	#endif
			return false;
		}

		public static bool ImportTexture( string path )
		{
	#if UNITY_EDITOR
			if( path.Length > 0 )
			{
				TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter; 
				if( textureImporter )
				{
                    textureImporter.alphaIsTransparency = true; // default
                    textureImporter.anisoLevel = 1; // default
                    textureImporter.borderMipmap = false; // default
                    textureImporter.mipmapEnabled = false; // default
                    textureImporter.compressionQuality = 100;
					textureImporter.isReadable = true;
					textureImporter.spritePixelsPerUnit = AutoTileset.PixelToUnits;                    
					textureImporter.spriteImportMode = SpriteImportMode.None;
					textureImporter.wrapMode = TextureWrapMode.Clamp;
					textureImporter.filterMode = FilterMode.Point;
					textureImporter.textureFormat = TextureImporterFormat.ARGB32;
                    textureImporter.textureType = TextureImporterType.Advanced;
					textureImporter.maxTextureSize = 2048;                    
					AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate); 
				}
				return true;
			}
	#endif
			return false;
		}
		
		private static void _CopyBuildingThumbnails( Texture2D _atlasTexture, Texture2D tilesetTex, int dstX, int dstY )
		{
			if( tilesetTex != null )
			{
				Rect srcRect = new Rect(0, 0, AutoTileset.TilePartWidth, AutoTileset.TilePartWidth);
				Rect dstRect = new Rect(0, 0, AutoTileset.TileWidth, AutoTileset.TileHeight);
				for( dstRect.y = dstY, srcRect.y = 0; dstRect.y < (dstY + 4*AutoTileset.TileHeight); dstRect.y += AutoTileset.TileHeight, srcRect.y += 2*AutoTileset.TileHeight )
				{
					for( dstRect.x = dstX, srcRect.x = 0; dstRect.x < dstX + AutoTileset.AutoTilesPerRow*AutoTileset.TileWidth; dstRect.x += AutoTileset.TileWidth, srcRect.x += 2*AutoTileset.TileWidth )
					{
						Color[] thumbnailPartColors;
						thumbnailPartColors = tilesetTex.GetPixels( Mathf.RoundToInt(srcRect.x), Mathf.RoundToInt(srcRect.y), Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height));
						_atlasTexture.SetPixels( Mathf.RoundToInt(dstRect.x), Mathf.RoundToInt(dstRect.y), Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height), thumbnailPartColors);
						
						thumbnailPartColors = tilesetTex.GetPixels( Mathf.RoundToInt(srcRect.x) + 3*AutoTileset.TilePartWidth, Mathf.RoundToInt(srcRect.y), Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height));
						_atlasTexture.SetPixels( Mathf.RoundToInt(dstRect.x)+AutoTileset.TilePartWidth, Mathf.RoundToInt(dstRect.y), Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height), thumbnailPartColors);
						
						thumbnailPartColors = tilesetTex.GetPixels( Mathf.RoundToInt(srcRect.x), Mathf.RoundToInt(srcRect.y) + 3*AutoTileset.TilePartHeight, Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height));
						_atlasTexture.SetPixels( Mathf.RoundToInt(dstRect.x), Mathf.RoundToInt(dstRect.y)+AutoTileset.TilePartHeight, Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height), thumbnailPartColors);
						
						thumbnailPartColors = tilesetTex.GetPixels( Mathf.RoundToInt(srcRect.x) + 3*AutoTileset.TilePartWidth, Mathf.RoundToInt(srcRect.y) + 3*AutoTileset.TilePartHeight, Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height));
						_atlasTexture.SetPixels( Mathf.RoundToInt(dstRect.x)+AutoTileset.TilePartWidth, Mathf.RoundToInt(dstRect.y)+AutoTileset.TilePartHeight, Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height), thumbnailPartColors);
						
					}
				}
			}
		}
		
		private static void _CopyWallThumbnails( Texture2D _atlasTexture, Texture2D tilesetTex, int dstX, int dstY )
		{
			if( tilesetTex != null )
			{
				Rect srcRect = new Rect(0, 3*AutoTileset.TileHeight, AutoTileset.TilePartWidth, AutoTileset.TilePartWidth);
				Rect dstRect = new Rect(0, 0, AutoTileset.TileWidth, AutoTileset.TileHeight);
				for( dstRect.y = dstY, srcRect.y = 0; dstRect.y < (dstY + 3*AutoTileset.TileHeight); dstRect.y += AutoTileset.TileHeight, srcRect.y += 5*AutoTileset.TileHeight )
				{
					for( dstRect.x = dstX, srcRect.x = 0; dstRect.x < dstX + AutoTileset.AutoTilesPerRow*AutoTileset.TileWidth; dstRect.x += AutoTileset.TileWidth, srcRect.x += 2*AutoTileset.TileWidth )
					{
						Color[] thumbnailPartColors;
						thumbnailPartColors = tilesetTex.GetPixels( Mathf.RoundToInt(srcRect.x), Mathf.RoundToInt(srcRect.y), Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height));
						_atlasTexture.SetPixels( Mathf.RoundToInt(dstRect.x), Mathf.RoundToInt(dstRect.y), Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height), thumbnailPartColors);
						
						thumbnailPartColors = tilesetTex.GetPixels( Mathf.RoundToInt(srcRect.x) + 3*AutoTileset.TilePartWidth, Mathf.RoundToInt(srcRect.y), Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height));
						_atlasTexture.SetPixels( Mathf.RoundToInt(dstRect.x)+AutoTileset.TilePartWidth, Mathf.RoundToInt(dstRect.y), Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height), thumbnailPartColors);
						
						thumbnailPartColors = tilesetTex.GetPixels( Mathf.RoundToInt(srcRect.x), Mathf.RoundToInt(srcRect.y) + 3*AutoTileset.TilePartHeight, Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height));
						_atlasTexture.SetPixels( Mathf.RoundToInt(dstRect.x), Mathf.RoundToInt(dstRect.y)+AutoTileset.TilePartHeight, Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height), thumbnailPartColors);
						
						thumbnailPartColors = tilesetTex.GetPixels( Mathf.RoundToInt(srcRect.x) + 3*AutoTileset.TilePartWidth, Mathf.RoundToInt(srcRect.y) + 3*AutoTileset.TilePartHeight, Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height));
						_atlasTexture.SetPixels( Mathf.RoundToInt(dstRect.x)+AutoTileset.TilePartWidth, Mathf.RoundToInt(dstRect.y)+AutoTileset.TilePartHeight, Mathf.RoundToInt(srcRect.width), Mathf.RoundToInt(srcRect.height), thumbnailPartColors);
						
					}
				}
			}
		}
		
		private static void _CopyTilesetInAtlas( Texture2D _atlasTexture, Texture2D tilesetTex, int dstX, int dstY, int width, int height )
		{
			Color[] atlasColors;
			if( tilesetTex == null )
			{
				atlasColors = Enumerable.Repeat<Color>( new Color(0f, 0f, 0f, 0f) , width*height).ToArray();
			}
			else
			{
				atlasColors = tilesetTex.GetPixels();
			}
			
			_atlasTexture.SetPixels( dstX, dstY, width, height, atlasColors);
		}
	}
}
