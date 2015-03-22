using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;

namespace CreativeSpore.RpgMapEditor
{
	[CustomEditor(typeof(AutoTileset))] 
	public class AutoTilesetEditor : Editor
	{
		AutoTileset MyAutoTileset{ get{ return (AutoTileset)target; } }

		string m_lastError = "";
		
		public override void OnInspectorGUI() 
		{
			// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
			serializedObject.Update();

			//Texture2D prevAtlasTexture = MyAutoTileset.TilesetsAtlasTexture;

			EditorGUILayout.HelpBox("Select a texture atlas directly or Generate a new atlas using separated textures", MessageType.Info);
			MyAutoTileset.TilesetsAtlasTexture = EditorGUILayout.ObjectField ("Tileset Atlas", MyAutoTileset.TilesetsAtlasTexture, typeof(Texture2D), false) as Texture2D;

			if( MyAutoTileset.TilesetsAtlasTexture == null )
			{
				EditorGUILayout.HelpBox( "Tileset textures", MessageType.Info );
				if( m_lastError.Length > 0 )
				{
					EditorGUILayout.HelpBox( m_lastError, MessageType.Error );
				}
				EditorGUILayout.BeginVertical( GUILayout.MinWidth(300));
				EditorGUILayout.LabelField("--- Animated (A1) ---");
				MyAutoTileset.AnimatedTexture = EditorGUILayout.ObjectField (MyAutoTileset.AnimatedTexture == null? "": MyAutoTileset.AnimatedTexture.name, MyAutoTileset.AnimatedTexture, typeof(Texture2D), false) as Texture2D;
				EditorGUILayout.LabelField("--- Ground (A2) ---");
				MyAutoTileset.GroundTexture = EditorGUILayout.ObjectField (MyAutoTileset.GroundTexture == null? "": MyAutoTileset.GroundTexture.name, MyAutoTileset.GroundTexture, typeof(Texture2D), false) as Texture2D;
				EditorGUILayout.LabelField("--- Building (A3) ---");
				MyAutoTileset.BuildingTexture = EditorGUILayout.ObjectField (MyAutoTileset.BuildingTexture == null? "": MyAutoTileset.BuildingTexture.name, MyAutoTileset.BuildingTexture, typeof(Texture2D), false) as Texture2D;
				EditorGUILayout.LabelField("--- Wall (A4) ---");
				MyAutoTileset.WallTexture = EditorGUILayout.ObjectField (MyAutoTileset.WallTexture == null? "": MyAutoTileset.WallTexture.name, MyAutoTileset.WallTexture, typeof(Texture2D), false) as Texture2D;
				EditorGUILayout.LabelField("--- Normal (A5) ---");
				MyAutoTileset.NormalTexture = EditorGUILayout.ObjectField (MyAutoTileset.NormalTexture == null? "": MyAutoTileset.NormalTexture.name, MyAutoTileset.NormalTexture, typeof(Texture2D), false) as Texture2D;
				EditorGUILayout.LabelField("--- Objects (B) ---");
				MyAutoTileset.ObjectsTexture_B = EditorGUILayout.ObjectField (MyAutoTileset.ObjectsTexture_B == null? "": MyAutoTileset.ObjectsTexture_B.name, MyAutoTileset.ObjectsTexture_B, typeof(Texture2D), false) as Texture2D;
				EditorGUILayout.LabelField("--- Objects (C) ---");
				MyAutoTileset.ObjectsTexture_C = EditorGUILayout.ObjectField (MyAutoTileset.ObjectsTexture_C == null? "": MyAutoTileset.ObjectsTexture_C.name, MyAutoTileset.ObjectsTexture_C, typeof(Texture2D), false) as Texture2D;
				EditorGUILayout.LabelField("--- Objects (D) ---");
				MyAutoTileset.ObjectsTexture_D = EditorGUILayout.ObjectField (MyAutoTileset.ObjectsTexture_D == null? "": MyAutoTileset.ObjectsTexture_D.name, MyAutoTileset.ObjectsTexture_D, typeof(Texture2D), false) as Texture2D;
				EditorGUILayout.LabelField("--- Objects (E) ---");
				MyAutoTileset.ObjectsTexture_E = EditorGUILayout.ObjectField (MyAutoTileset.ObjectsTexture_E == null? "": MyAutoTileset.ObjectsTexture_E.name, MyAutoTileset.ObjectsTexture_E, typeof(Texture2D), false) as Texture2D;
				EditorGUILayout.EndVertical();

				//+++ Check all textures have right dimensions
				if( GUI.changed )
				{
					MyAutoTileset.TilesetsAtlasTexture = _ValidateTilesetTexture( MyAutoTileset.TilesetsAtlasTexture, 2048, 2048 );
					MyAutoTileset.AnimatedTexture = _ValidateTilesetTexture( MyAutoTileset.AnimatedTexture, 512, 384 );
					MyAutoTileset.GroundTexture = _ValidateTilesetTexture( MyAutoTileset.GroundTexture, 512, 384 );
					MyAutoTileset.BuildingTexture = _ValidateTilesetTexture( MyAutoTileset.BuildingTexture, 512, 256 );
					MyAutoTileset.WallTexture = _ValidateTilesetTexture( MyAutoTileset.WallTexture, 512, 480 );
					MyAutoTileset.NormalTexture = _ValidateTilesetTexture( MyAutoTileset.NormalTexture, 256, 512 );
					MyAutoTileset.ObjectsTexture_B = _ValidateTilesetTexture( MyAutoTileset.ObjectsTexture_B, 512, 512 );
					MyAutoTileset.ObjectsTexture_C = _ValidateTilesetTexture( MyAutoTileset.ObjectsTexture_C, 512, 512 );
					MyAutoTileset.ObjectsTexture_D = _ValidateTilesetTexture( MyAutoTileset.ObjectsTexture_D, 512, 512 );
					MyAutoTileset.ObjectsTexture_E = _ValidateTilesetTexture( MyAutoTileset.ObjectsTexture_E, 512, 512 );
					EditorUtility.SetDirty( MyAutoTileset );
				}
				//---

				if( GUILayout.Button( "Generate Atlas") )
				{
					m_lastError = "";
					Texture2D atlasTexture = UtilsAutoTileMap.GenerateAtlas( MyAutoTileset );
					if( atlasTexture )
					{
						string path = Path.GetDirectoryName( AssetDatabase.GetAssetPath(MyAutoTileset) );
						string filePath = EditorUtility.SaveFilePanel( "Save Atlas",path,MyAutoTileset.name + ".png", "png");
						if( filePath.Length > 0 )
						{
							byte[] bytes = atlasTexture.EncodeToPNG();
							File.WriteAllBytes(filePath, bytes);

							// make path relative to project
							filePath = "Assets" + filePath.Remove(0, Application.dataPath.Length);

							// Make sure LoadAssetAtPath and ImportTexture is going to work
							AssetDatabase.Refresh();

							UtilsAutoTileMap.ImportTexture( filePath );

							// Link Atlas with asset to be able to save it in the prefab
							MyAutoTileset.TilesetsAtlasTexture = (Texture2D)AssetDatabase.LoadAssetAtPath( filePath, typeof(Texture2D));
							UtilsAutoTileMap.ImportTexture( MyAutoTileset.TilesetsAtlasTexture );
						}
						else
						{
							MyAutoTileset.TilesetsAtlasTexture = null;
						}
					}

					EditorUtility.SetDirty( MyAutoTileset );
					m_lastError = "";
				}
			}
			else
			{
				if( GUI.changed )
				{
					m_lastError = "";
					MyAutoTileset.TilesetsAtlasTexture = _ValidateTilesetTexture( MyAutoTileset.TilesetsAtlasTexture, 2048, 2048 );
					EditorUtility.SetDirty( MyAutoTileset );
				}

				if( GUILayout.Button( "Edit Tileset...") )
				{
					AutoTilesetEditorWindow.ShowDialog( MyAutoTileset );
				}
				GUILayout.Space( 20 );

				if( GUILayout.Button( "Remove Atlas") )
				{
					MyAutoTileset.TilesetsAtlasTexture = null;
					EditorUtility.SetDirty( MyAutoTileset );
				}
			}

            /* not necessary because GenerateAutoTileData will be called when setting TilesetsAtlasTexture
            if( prevAtlasTexture == null && MyAutoTileset.TilesetsAtlasTexture != null )
            {
                MyAutoTileset.GenerateAutoTileData();
            }
            */

			// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
			serializedObject.ApplyModifiedProperties();
		}

		private Texture2D _ValidateTilesetTexture( Texture2D texture, int width, int height )
		{
			if( texture != null )
			{
				if( texture.width == width && texture.height == height )
				{
					return texture;
				}
				else
				{
					m_lastError = (texture.name+" should be "+width+"x"+height+" but it is "+texture.width+"x"+texture.height);
				}
			}
			return null;
		}	
	}
}