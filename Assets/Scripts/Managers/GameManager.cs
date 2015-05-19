using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(NotificationManager))]
public class GameManager : MonoBehaviour
{
     public static GameManager Instance
     {
          get
          {
               if (instance == null) instance = new GameObject("GameManager").AddComponent<GameManager>();
               return instance;
          }
     }

     public static NotificationManager Notifications
     {
          get
          {
               if (notifications == null)
               {
                    notifications = instance.GetComponent<NotificationManager>();
               }
               return notifications;
          }
     }

     public static LoadAndSaveManager StateManager
     {
          get
          {
               if (stateManager == null)
               {
                    stateManager = instance.GetComponent<LoadAndSaveManager>();

               }
               return stateManager;
          }
     }
     void Awake()
     {
          if ((instance) && (instance.GetInstanceID() != GetInstanceID()))
          {
               DestroyImmediate(gameObject);
          }
          else
          {
               instance = this;
               DontDestroyOnLoad(gameObject);
          }
     }

     private static GameManager instance = null;
     private static NotificationManager notifications = null;
     private static LoadAndSaveManager stateManager = null;
     private static int currentLevel;

     public static int Coins;
     public static int Score;
     public static float timeToCompleteLevel;

     // Use this for initialization
     void Start()
     {
          currentLevel = Application.loadedLevel;
          Coins = 0;
          LoadGameData();
          GameManager.Notifications.AddListener(this, "EndOfLevelReached");

     }

     public static void AddPoints(int added)
     {
          Coins += added;
     }

     public static void SubtractPoints(int subtract)
     {
          Coins -= subtract;
     }

     public void Reset()
     {
          Coins = 0;
     }

     public static void setPoints(int coinsSet)
     {
          Coins = coinsSet;
     }

     public static int getCoins()
     {
          return Coins;
     }

     public static int getScore()
     {
          return Score;
     }

     public static float getTime()
     {
          timeToCompleteLevel = Time.time;
          if(timeToCompleteLevel > 60)
          {
               timeToCompleteLevel = timeToCompleteLevel / 60;
          }
          return timeToCompleteLevel;
     }


     public void LoadGameData()
     {
          StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");
     }

     public void EndOfLevelReached()
     {
          CalculateScore();
          LevelComplete();
          SaveGame();
     }

     public static void LevelComplete()
     {
          bool isActive = StateManager.isActiveAndEnabled;
          if (stateManager.GameState.GameLevels.Count >= currentLevel)
          {
               if (stateManager.GameState.GameLevels[currentLevel - 1].Score < Score)
               {
                    stateManager.GameState.GameLevels[currentLevel - 1].Score = Score;
               }
          }
          else
          {
               LoadAndSaveManager.GameStateData.GameLevelData newLevel = new LoadAndSaveManager.GameStateData.GameLevelData();
               newLevel.LevelUnlocked = true;
               newLevel.Score = Score;
               stateManager.GameState.GameLevels.Add(newLevel);
          }
     }

     public void CalculateScore()
     {
          timeToCompleteLevel = Time.time;
          Score = (int)Math.Round(Coins - timeToCompleteLevel);


     }

     public void SaveGame()
     {
          StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
     }


     public void SwitchLevel(int level)
     {
          Application.LoadLevel(level);
     }

     public void QuitGame()
     {
          Application.Quit();
     }
}
