﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CreativeSpore.RpgMapEditor
{
	public class TileChunk : MonoBehaviour 
	{
		private Vector3[] m_vertices;
		private Vector2[] m_uv;
		private int[] m_triangles;

		public AutoTileMap MyAutoTileMap;

		public int TileWidth = 8; // width size of chunk
        public int TileHeight = 4; // height size of chunk
		public int TileLayer = 0;
		public int StartTileX = 0;
		public int StartTileY = 0;

		private MeshFilter m_meshFilter;

		struct AnimTileData
		{
			public int VertexIdx;
			public float U0,U1;
            public float V0, V1; // used for waterfalls
            public int SubTileRow; // used for waterfalls
		}
		private List<AnimTileData> m_animatedTiles = new List<AnimTileData>();
        private List<AnimTileData> m_animatedWaterfallTiles = new List<AnimTileData>();

		public void OnWillRenderObject()
		{
			if( MyAutoTileMap.TileAnimFrameHasChanged )
			{
				m_uv = m_meshFilter.mesh.uv;

                // Animated tiles
				float fTextTileWidth = (float)AutoTileset.TileWidth / MyAutoTileMap.Tileset.TilesetsAtlasTexture.width;
				float offset = fTextTileWidth * MyAutoTileMap.TileAnim3Frame * 2;
				foreach( AnimTileData animTileData in m_animatedTiles )
				{
					m_uv[ animTileData.VertexIdx + 0 ].x = animTileData.U0 + offset;
					m_uv[ animTileData.VertexIdx + 1 ].x = animTileData.U0 + offset;
					m_uv[ animTileData.VertexIdx + 2 ].x = animTileData.U1 + offset;
					m_uv[ animTileData.VertexIdx + 3 ].x = animTileData.U1 + offset;
				}
                // waterfall tiles
                float fTextTilePartHeight = (float)AutoTileset.TilePartHeight / MyAutoTileMap.Tileset.TilesetsAtlasTexture.height;
                foreach (AnimTileData animTileData in m_animatedWaterfallTiles)
                {
                    int tilePartOff = (animTileData.SubTileRow + 4-MyAutoTileMap.TileAnim4Frame) % 4 - animTileData.SubTileRow;
                    offset = -fTextTilePartHeight * tilePartOff;
                    m_uv[animTileData.VertexIdx + 0].y = animTileData.V0 + offset;
                    m_uv[animTileData.VertexIdx + 1].y = animTileData.V1 + offset;
                    m_uv[animTileData.VertexIdx + 2].y = animTileData.V1 + offset;
                    m_uv[animTileData.VertexIdx + 3].y = animTileData.V0 + offset;
                }
				m_meshFilter.mesh.uv = m_uv;
			}
		}

		public void Configure (AutoTileMap autoTileMap, int layer, int startTileX, int startTileY, int tileChunkWidth, int tileChunkHeight)
		{
			MyAutoTileMap = autoTileMap;
			TileWidth = tileChunkWidth;
			TileHeight = tileChunkHeight;
			TileLayer = layer;
			StartTileX = startTileX;
			StartTileY = startTileY;

			transform.gameObject.name = "TileChunk"+startTileX+""+startTileY;
			Vector3 vPosition = new Vector3();
			vPosition.x = startTileX * AutoTileset.TileWorldWidth;
			vPosition.y = -startTileY * AutoTileset.TileWorldHeight;
			transform.localPosition = vPosition;

			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			if( meshRenderer == null )
			{
				meshRenderer = transform.gameObject.AddComponent<MeshRenderer>();
			}
			meshRenderer.sharedMaterial = MyAutoTileMap.Tileset.AtlasMaterial;
			meshRenderer.castShadows = false;
			meshRenderer.receiveShadows = false;

            m_meshFilter = GetComponent<MeshFilter>();
            if (m_meshFilter == null)
            {
                m_meshFilter = transform.gameObject.AddComponent<MeshFilter>();
            }
		}

		public void ApplyData()
		{

            Mesh mesh;
            if (!Application.isPlaying)
            {
                // During edit mode, sharedMesh is null ( I believe it was not created yet ) 
                // and getting mesh force it to be created and an error warning to be displayed
                // So during edit mode, a mesh should be created by hand in this way to avoid log errors
                if (m_meshFilter.sharedMesh != null)
                {
                    mesh = m_meshFilter.sharedMesh;
                }
                else
                {
                    mesh = new Mesh();
                    m_meshFilter.mesh = mesh;
                }
            }
            else
            {
                mesh = m_meshFilter.mesh;
            }
            mesh.Clear();
			
			FillData();

            mesh.vertices = m_vertices;
            mesh.uv = m_uv;
            mesh.triangles = m_triangles;
		}

		void FillData()
		{
			m_animatedTiles.Clear();
            m_animatedWaterfallTiles.Clear();
			m_vertices = new Vector3[ TileWidth*TileHeight*4*4 ]; // 4 subtiles x 4 vertex per tile
			m_uv = new Vector2[ m_vertices.Length ];
			m_triangles = new int[ TileWidth*TileHeight*4*2*3 ]; // 4 subtiles x 2 triangles per tile x 3 vertex per triangle

			int vertexIdx = 0;
			int triangleIdx = 0;
			int SubTileWidth = TileWidth * 2;
			int SubTileHeight = TileHeight * 2;
			for( int subTileX = 0; subTileX < SubTileWidth; ++subTileX )
			{
				for( int subTileY = 0; subTileY < SubTileHeight; ++subTileY )
				{
					int tileX = StartTileX + subTileX/2;
					int tileY = StartTileY + subTileY/2;
					AutoTileMap.AutoTile autoTile = MyAutoTileMap.GetAutoTile( tileX, tileY, TileLayer);
					if( autoTile.Type >= 0 )
					{
						float px0 = subTileX*(AutoTileset.TileWorldWidth/2);
						float py0 = -subTileY*(AutoTileset.TileWorldHeight/2);
						float px1 = (subTileX+1)*(AutoTileset.TileWorldWidth/2);
						float py1 = -(subTileY+1)*(AutoTileset.TileWorldHeight/2);

						m_vertices[ vertexIdx + 0 ] = new Vector3( px0, py0, 0 );
						m_vertices[ vertexIdx + 1 ] = new Vector3( px0, py1, 0 );
						m_vertices[ vertexIdx + 2 ] = new Vector3( px1, py1, 0 );
						m_vertices[ vertexIdx + 3 ] = new Vector3( px1, py0, 0 );

						m_triangles[ triangleIdx + 0 ] = vertexIdx + 2;
						m_triangles[ triangleIdx + 1 ] = vertexIdx + 1;
						m_triangles[ triangleIdx + 2 ] = vertexIdx + 0;
						m_triangles[ triangleIdx + 3 ] = vertexIdx + 0;
						m_triangles[ triangleIdx + 4 ] = vertexIdx + 3;
						m_triangles[ triangleIdx + 5 ] = vertexIdx + 2;

						float u0, u1, v0, v1;
						if( autoTile.Type >= 128 )
						{
							int spriteIdx = autoTile.TilePartsIdx[ 0 ];
							Sprite sprTile = MyAutoTileMap.Tileset.AutoTileSprites[spriteIdx];
                            u0 = (((subTileX % 2) * sprTile.rect.width / 2) + sprTile.rect.x) / sprTile.texture.width;
                            u1 = (((subTileX % 2) * sprTile.rect.width / 2) + sprTile.rect.x + sprTile.rect.width / 2 - 1f) / sprTile.texture.width;
                            v0 = (((1 - subTileY % 2) * sprTile.rect.height / 2) + sprTile.rect.y + sprTile.rect.height / 2 - 1f) / sprTile.texture.height;
                            v1 = (((1 - subTileY % 2) * sprTile.rect.height / 2) + sprTile.rect.y) / sprTile.texture.height;
						}
						else
						{
							int tilePartIdx = (subTileY%2)*2 + (subTileX%2);
							int spriteIdx = autoTile.TilePartsIdx[ tilePartIdx ];
							Sprite sprTile = MyAutoTileMap.Tileset.AutoTileSprites[spriteIdx];
                            u0 = sprTile.rect.x / sprTile.texture.width;
                            u1 = (sprTile.rect.x + sprTile.rect.width - 1f) / sprTile.texture.width;
                            v0 = (sprTile.rect.y + sprTile.rect.height - 1f) / sprTile.texture.height;
                            v1 = sprTile.rect.y / sprTile.texture.height;

                            if (MyAutoTileMap.Tileset.IsAutoTileAnimated(autoTile.Type))
                            {
                                m_animatedTiles.Add(new AnimTileData() { VertexIdx = vertexIdx, U0 = u0, U1 = u1 });
                            }
                            else if (MyAutoTileMap.Tileset.IsAutoTileAnimatedWaterfall(autoTile.Type))
                            {
                                int subTileRow = (spriteIdx / 32);//32 = number of subtiles in a row;
                                subTileRow %= 6; // make it relative to this autotile
                                subTileRow -= 2; // remove top tiles
                                m_animatedWaterfallTiles.Add(new AnimTileData() { VertexIdx = vertexIdx, V0 = v0, V1 = v1, SubTileRow = subTileRow });
                            }
						}
						m_uv[ vertexIdx + 0 ] = new Vector3(u0, v0, 0);
						m_uv[ vertexIdx + 1 ] = new Vector3(u0, v1, 0);
						m_uv[ vertexIdx + 2 ] = new Vector3(u1, v1, 0);
						m_uv[ vertexIdx + 3 ] = new Vector3(u1, v0, 0);

						// increment vectex and triangle idx
						vertexIdx+=4; 
						triangleIdx+=6;
					}
				}
			}

			// resize arrays
			System.Array.Resize( ref m_vertices, vertexIdx );
			System.Array.Resize( ref m_uv, vertexIdx );
			System.Array.Resize( ref m_triangles, triangleIdx );
		}
	}
}