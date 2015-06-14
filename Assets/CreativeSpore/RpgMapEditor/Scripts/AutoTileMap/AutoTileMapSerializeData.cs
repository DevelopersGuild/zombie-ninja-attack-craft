using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace CreativeSpore.RpgMapEditor
{
    /// <summary>
    /// Map data containing all tiles and the size of the map
    /// </summary>
	[System.Serializable, XmlRoot("AutoTileMap")]    
	public class AutoTileMapSerializeData 
	{
		[System.Serializable]
		public class MetadataChunk
		{
			public string version = "1.2.2"; //TODO: change this after each update!
			public bool compressedTileData = true;
		}

		[System.Serializable]
		public class TileLayer
		{
			public List<int> Tiles;
		}

		public MetadataChunk Metadata = new MetadataChunk(); 
		public int TileMapWidth;
		public int TileMapHeight;
		public List<TileLayer> TileData = new List<TileLayer>((int)AutoTileMap.eTileLayer._SIZE);

	 	public void CopyData (AutoTileMapSerializeData mapData)
		{
			Metadata = mapData.Metadata;
			TileMapWidth = mapData.TileMapWidth;
			TileMapHeight = mapData.TileMapHeight;
			TileData = mapData.TileData;
		}

        /// <summary>
        /// Save the map configuration
        /// </summary>
        /// <param name="_autoTileMap"></param>
        /// <returns></returns>
		public bool SaveData( AutoTileMap _autoTileMap )
		{
            // avoid clear map data when auto tile map is not initialized
			if( !_autoTileMap.IsInitialized )
			{
				//Debug.LogError(" Error saving data. Autotilemap is not initialized! Map will not be saved. ");
				return false;
			}

			TileMapWidth = _autoTileMap.MapTileWidth;
			TileMapHeight = _autoTileMap.MapTileHeight;
			TileData.Clear();
			for( int iLayer = 0; iLayer < (int)AutoTileMap.eTileLayer._SIZE; ++iLayer )
			{
				List<int> tileData = new List<int>(TileMapWidth*TileMapHeight);
				int iTileRepetition = 0;
				int savedType = 0;
				for( int tile_y = 0; tile_y < TileMapHeight; ++tile_y )
				{
					for( int tile_x = 0; tile_x < TileMapWidth; ++tile_x )
					{
						int iType = _autoTileMap.GetAutoTile( tile_x, tile_y, iLayer ).Idx;

						if( iTileRepetition == 0 )
						{
							savedType = iType;
							iTileRepetition = 1;
						}
						else
						{
							// compression data. All tiles of the same type are store with number of repetitions ( negative number ) and type
							// ex: 5|5|5|5 --> |-4|5| (4 times 5) ex: -1|-1|-1 --> |-3|-1| ( 3 times -1 )
							if( iType == savedType ) ++iTileRepetition;
							else
							{
								if( iTileRepetition > 1 )
								{
									tileData.Add( -iTileRepetition );
								}
								tileData.Add( savedType );
								savedType = iType;
								iTileRepetition = 1;
							}
						}
					}
				}
				// save last tile type found
				if( iTileRepetition > 1 )
				{
					tileData.Add( -iTileRepetition );
				}
				tileData.Add( savedType );

				// 
				TileData.Add( new TileLayer(){ Tiles = tileData } );
			}
			return true;
		}

        /// <summary>
        /// Get this object serialized as an xml string
        /// </summary>
        /// <returns></returns>
		public string GetXmlString()
		{
			return UtilsSerialize.Serialize<AutoTileMapSerializeData>(this);
		}

        /// <summary>
        /// Save this object serialized in an xml file
        /// </summary>
        /// <param name="_filePath"></param>
		public void SaveToFile(string _filePath)
		{
			var serializer = new XmlSerializer(typeof(AutoTileMapSerializeData));
			var stream = new FileStream(_filePath, FileMode.Create);
			serializer.Serialize(stream, this);
			stream.Close();
		}

        /// <summary>
        /// Create map serialized data from xml file
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
		public static AutoTileMapSerializeData LoadFromFile(string _filePath)
		{
			var serializer = new XmlSerializer(typeof(AutoTileMapSerializeData));
			var stream = new FileStream(_filePath, FileMode.Open);
			var obj = serializer.Deserialize(stream) as AutoTileMapSerializeData;
			stream.Close();
			return obj;
		}

        /// <summary>
        /// Create map serialized data from xml string
        /// </summary>
        /// <param name="_xml"></param>
        /// <returns></returns>
		public static AutoTileMapSerializeData LoadFromXmlString(string _xml)
		{
			return UtilsSerialize.Deserialize<AutoTileMapSerializeData>(_xml);
		}

        /// <summary>
        /// Load map serialized data into a map
        /// </summary>
        /// <param name="_autoTileMap"></param>
		public void LoadToMap( AutoTileMap _autoTileMap )
		{
			_autoTileMap.Initialize();
			_autoTileMap.ClearMap();
			for( int iLayer = 0; iLayer < TileData.Count; ++iLayer )
			{
				int iTileRepetition = 1;
				int iTileIdx = 0;
				foreach( int iType in TileData[iLayer].Tiles )
				{
					//see compression notes in CreateFromTilemap
					if( iType < -1 )
					{
						iTileRepetition = -iType;
					}
					else
					{
						for( ;iTileRepetition > 0; --iTileRepetition, ++iTileIdx )
						{
							int tile_x = iTileIdx % TileMapWidth;
							int tile_y = iTileIdx / TileMapWidth;
							if( iType >= 0 )
							{
								_autoTileMap.SetAutoTile( tile_x, tile_y, iType, iLayer, false);
							}
						}
						iTileRepetition = 1;
					}
				}
			}
            _autoTileMap.RefreshAllTiles();
			_autoTileMap.RefreshMinimapTexture();
		}
	}
}
