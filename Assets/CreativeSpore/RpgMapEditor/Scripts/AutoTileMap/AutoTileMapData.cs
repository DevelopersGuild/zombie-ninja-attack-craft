using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace CreativeSpore.RpgMapEditor
{
	//NOTE: I had to split this class to avoid ScriptableObject warning while deserializing
	public class AutoTileMapData : ScriptableObject 
	{
		public AutoTileMapSerializeData Data;
	}
}
