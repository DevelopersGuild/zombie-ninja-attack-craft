using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class LoadAndSaveManager : MonoBehaviour
{
     public class GameStateData
     {
          public class GameLevelData
          {
               public bool LevelUnlocked;
               public int PlayerScoreCombat;
               public int PlayerScoreTime;
               public int GoldScoreCombat;
               public int GoldScoreTime;
          }

          public class PlayerData
          {
               public bool IsBowUnlocked;
               public bool IsBowHoldDownUnlocked;
               public bool IsLandMineUnlocked;
               public int StartingHealth;
               public bool IsDashUnlocked;
               public float DashSpeed;
          }
          public List<GameLevelData> GameLevels = new List<GameLevelData>();

          public PlayerData Player = new PlayerData();
     }

     public GameStateData GameState = new GameStateData();

     public void Save(string FileName = "GameData.xml")
     {
          var encoding = Encoding.GetEncoding("UTF-8");
          XmlSerializer Serializer = new XmlSerializer(typeof(GameStateData));
          StreamWriter FStream = new StreamWriter(FileName, false, encoding);
          Serializer.Serialize(FStream, GameState);
          FStream.Close();
     }

     public void Load(string FileName = "GameData.xml")
     {
          var encoding = Encoding.GetEncoding("UTF-8");
          if(File.Exists(Application.persistentDataPath + "/SaveGame.xml") == false)
          {
               Save(Application.persistentDataPath + "/SaveGame.xml");
          }
          XmlSerializer Serializer = new XmlSerializer(typeof(GameStateData));
          try
          {
               StreamReader FStream = new StreamReader(FileName);
               GameState = Serializer.Deserialize(FStream) as GameStateData;
               FStream.Close();
          }

          catch (FileNotFoundException)
          {

          }
     }
}
