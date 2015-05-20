using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class LoadAndSaveManager : MonoBehaviour
{
     public class GameStateData
     {
          public class GameLevelData
          {
               public bool LevelUnlocked;
               public int Score;
          }

          public class PlayerData
          {
               public bool BowUnlocked;

          }
          public List<GameLevelData> GameLevels = new List<GameLevelData>();

          public PlayerData Player = new PlayerData();
     }

     public GameStateData GameState = new GameStateData();

     public void Save(string FileName = "GameData.xml")
     {
          XmlSerializer Serializer = new XmlSerializer(typeof(GameStateData));
          FileStream FStream = new FileStream(FileName, FileMode.Create);
          Serializer.Serialize(FStream, GameState);
          FStream.Close();
     }

     public void Load(string FileName = "GameData.xml")
     {
          XmlSerializer Serializer = new XmlSerializer(typeof(GameStateData));
          try
          {
               FileStream FStream = new FileStream(FileName, FileMode.Open);
               GameState = Serializer.Deserialize(FStream) as GameStateData;
               FStream.Close();
          }

          catch (FileNotFoundException)
          {

          }
     }
}
