using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CreativeSpore.RpgMapEditor
{
	public class RpgMapMakerEditor : EditorWindow 
	{
		[MenuItem ("Assets/Create/RpgMapEditor/AutoTileset")]
		public static AutoTileset CreateTileset() 
		{
			string assetPath = GetUniqueAssetPath("AutoTileset");

			if( string.IsNullOrEmpty( assetPath ) )
			{
				return null;
			}
			else
			{
				AutoTileset autoTileSet = ScriptableObject.CreateInstance<AutoTileset>();
				AssetDatabase.CreateAsset( autoTileSet, assetPath );
				AssetDatabase.Refresh();
				return autoTileSet;
			}
		}

		public static AutoTileMapData CreateAutoTileMapData() 
		{
			string assetPath = GetUniqueAssetPath("AutoTileMapData");

			if( string.IsNullOrEmpty( assetPath ) )
			{
				return null;
			}
			else
			{
				AutoTileMapData autoTileMapData = ScriptableObject.CreateInstance<AutoTileMapData>();
				autoTileMapData.Data = new AutoTileMapSerializeData();
				autoTileMapData.Data.TileMapWidth = 200;
				autoTileMapData.Data.TileMapHeight = 200;
				
				AssetDatabase.CreateAsset( autoTileMapData, assetPath );
				AssetDatabase.Refresh();
				return autoTileMapData;
			}
		}

		[MenuItem("GameObject/Create Other/RpgMapEditor/AutoTileMap")]
		public static void CreateAutoTileMap() 
		{
			GameObject objTilemap = new GameObject();
			objTilemap.name = "AutoTileMap";
			objTilemap.AddComponent<AutoTileMap>();
		}

		public static string GetUniqueAssetPath( string name )
		{
			Object obj = Selection.activeObject;
			string assetPath = AssetDatabase.GetAssetPath(obj);
			if (assetPath.Length == 0)		
			{
				assetPath = EditorUtility.SaveFilePanel( "Save asset prefab",	"",	name + ".prefab",	"prefab");
				string cwd = System.IO.Directory.GetCurrentDirectory().Replace("\\","/") + "/assets/";
				if (assetPath.ToLower().IndexOf(cwd.ToLower()) != 0)
				{
					EditorUtility.DisplayDialog("Error saving asset", "You must save the asset inside the Asset Folder", "Ok");
				}
				else 
				{
					assetPath = assetPath.Substring(cwd.Length - "/assets".Length);
				}
			}
			else
			{
				// is a directory
				string path = System.IO.Directory.Exists(assetPath) ? assetPath : System.IO.Path.GetDirectoryName(assetPath);
				assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".prefab");
			}
			return assetPath;
		}
	}
}