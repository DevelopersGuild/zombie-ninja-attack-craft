using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace CreativeSpore.RpgMapEditor
{
	public class RpgMapMakerEditor : EditorWindow 
	{
		[MenuItem ("Assets/Create/RpgMapEditor/AutoTileset")]
		public static AutoTileset CreateTileset() 
		{
            /* old way, by opening save file dialog
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
            */

            return CreateAssetInSelectedDirectory<AutoTileset>();
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

		[MenuItem("GameObject/RpgMapEditor/AutoTileMap", false, 10)]
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

        public static T CreateAssetInSelectedDirectory<T>() where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + Path.GetExtension(typeof(T).ToString()).Remove(0, 1) + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            return asset;
        }
	}
}