using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(NotificationManager))]
public class GameManager : MonoBehaviour
{
     //Get Components
     public static GameManager Instance
     {
          get
          {
               if (instance == null)
               {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
               }
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

     //Variables

     private static GameManager instance = null;
     private static NotificationManager notifications = null;
     private static LoadAndSaveManager stateManager = null;
     public static int CurrentLevel;

     public static int Coins;
     public static int Score;
     public static int deaths;
     public static int killCount;
     public static float timeToCompleteLevel;
     public static bool IsCurrentLevelComplete = false;
     public int PassingScore = 0;
     public bool UnlockAllUnlocks = false;
     public bool ResetUnlocks = false;

     public bool hasSentFeedback = false;

     // Use this for initialization
     void Start()
     {
          CurrentLevel = Application.loadedLevel;
          OnLevelWasLoaded(CurrentLevel);
          if (UnlockAllUnlocks == true)
          {
               UnlockEverything();
          }
          if(ResetUnlocks == true)
          {
               ResetGameProgression();
          }
          GameManager.Notifications.AddListener(this, "EndOfLevelReached");

     }

     public void OnLevelWasLoaded(int level)
     {
          Coins = 0;
          Score = 0;
          killCount = 0;
          IsCurrentLevelComplete = false;
          timeToCompleteLevel = 0;
          LoadGameData();
          GameManager.Notifications.PostNotification(this, "LevelLoaded");
          UnpauseGame();
     }

     public void LoadGameData()
     {
          StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");
     }


     //Coin methods
     public static void AddCoins(int added)
     {
          Coins += added;
     }

     public static void SubtractCoins(int subtract)
     {
          Coins -= subtract;
     }

     public void Reset()
     {
          Coins = 0;
     }

     public static void setCoins(int coinsSet)
     {
          Coins = coinsSet;
     }

     public static int getCoins()
     {
          return Coins;
     }


     //Time methods
     public static float getTime()
     {
          timeToCompleteLevel = Time.time;
          return timeToCompleteLevel;
     }

     public static string getTimeFormatted()
     {
          int minutes;
          int seconds;

          float time = GameManager.getTime();
          if (time > 60)
          {
               minutes = (int)time / 60;
               seconds = (int)time % 60;
               return  minutes + "minutes and " + seconds + "seconds";
          }
          else
          {
               seconds = (int)time;
               return seconds + "seconds";
          }
     }


     //Score methods
     public static int getScore()
     {
          return Score;
     }

     public static void incrementKills()
     {
          killCount++;
     }

     public static int getKills()
     {
          return killCount;
     }
     public static void setKills(int kills)
     {
          killCount = kills;
     }

     public static void incrementDeaths()
     {
          deaths++;
          Debug.Log(deaths);
     }

     public static int getDeaths()
     {
          return deaths;
     }

     public static void setDeaths(int death)
     {
          deaths = death;
     }

     public static bool getIsLevelComplete()
     {
          return IsCurrentLevelComplete;
     }

     public void EndOfLevelReached()
     {
          CalculateScore();
          LevelComplete();
          Debug.Log("score:" + GameManager.getScore().ToString());
          GameManager.Notifications.PostNotification(this, "PrepareToSave");
          SaveGame();
          Debug.Log("score:" + GameManager.getScore().ToString());
          GameManager.Notifications.PostNotification(this, "ScoreReadyToDisplay");
          Debug.Log("woo!");
     }

     public void CalculateScore()
     {
          timeToCompleteLevel = Time.time;
          Score = (int)Math.Round(Coins - timeToCompleteLevel);

     }

     //Level unlock methods

     /*
     public int GetGoldCombatScore(int level)
     {
          return (stateManager.GameState.GameLevels.Count <= level) ? stateManager.GameState.GameLevels[level - 1].GoldScoreCombat : 0;
     }

     public int GetGoldTimeScore(int level)
     {
          return (stateManager.GameState.GameLevels.Count <= level) ? stateManager.GameState.GameLevels[level - 1].GoldScoreTime : 0;
     }
      * */

     public int GetPlayerCombatScore(int level)
     {
          return (stateManager.GameState.GameLevels.Count > level) ? stateManager.GameState.GameLevels[level - 1].PlayerScoreCombat : 0;
     }

     public float GetPlayerTimeScore(int level)
     {
          return (stateManager.GameState.GameLevels.Count > level) ? stateManager.GameState.GameLevels[level - 1].PlayerScoreTime : 0;
     }


     public static void LevelComplete()
     {
          bool isActive = StateManager.isActiveAndEnabled;
          if (CurrentLevel == 0)
          {
               //Happens while choosing playing a level scene directly from within the Unity Editor.
               Debug.Log("Not saving score on level because I don't know which level number this is");
               return;
          }
          if (stateManager.GameState.GameLevels.Count >= CurrentLevel)
          {
               if (stateManager.GameState.GameLevels[CurrentLevel - 1].PlayerScoreCombat < Score)
               {
                    stateManager.GameState.GameLevels[CurrentLevel - 1].PlayerScoreCombat = Score;
               }
               if(stateManager.GameState.GameLevels[CurrentLevel-1].PlayerScoreTime == 0 || stateManager.GameState.GameLevels[CurrentLevel -1].PlayerScoreTime > timeToCompleteLevel)
               {
                    stateManager.GameState.GameLevels[CurrentLevel - 1].PlayerScoreTime = timeToCompleteLevel;
               }
          }
          else
          {
               LoadAndSaveManager.GameStateData.GameLevelData newLevel = new LoadAndSaveManager.GameStateData.GameLevelData();
               newLevel.LevelUnlocked = true;
               newLevel.PlayerScoreCombat = Score;
               newLevel.PlayerScoreTime = Time.time;
               stateManager.GameState.GameLevels.Add(newLevel);
          }
     }


     public void SaveGame()
     {
          GameManager.Notifications.PostNotification(this, "PrepareToSave");
          StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
     }


     //Game state controls
     public void SwitchLevel(int level)
     {
          Application.LoadLevel(level);
     }

     public void QuitGame()
     {
          Application.Quit();
     }

     public void PauseGame()
     {
          Time.timeScale = 0;
     }

     public void UnpauseGame()
     {
          Time.timeScale = 1.0f;
     }

     //Player Progression

     public void ResetGameProgression()
     {
          foreach (LoadAndSaveManager.GameStateData.GameLevelData level in stateManager.GameState.GameLevels)
          {
               level.LevelUnlocked = false;
               level.PlayerScoreCombat = 0;
          }
          stateManager.GameState.Player.IsBowHoldDownUnlocked = false;
          stateManager.GameState.Player.IsBowUnlocked = false;
          stateManager.GameState.Player.IsDashUnlocked = false;
          stateManager.GameState.Player.DashSpeed = 0;
          stateManager.GameState.Player.IsLandMineUnlocked = false;
          stateManager.GameState.Player.StartingHealth = 0;
          StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
          GameManager.Notifications.PostNotification(this, "LevelLoaded");
     }

     public void UnlockEverything()
     {
          foreach (LoadAndSaveManager.GameStateData.GameLevelData level in stateManager.GameState.GameLevels)
          {
               level.LevelUnlocked = true;
          }
          stateManager.GameState.Player.IsBowHoldDownUnlocked = true;
          stateManager.GameState.Player.IsBowUnlocked = true;
          stateManager.GameState.Player.IsDashUnlocked = true;
          stateManager.GameState.Player.DashSpeed = 0;
          stateManager.GameState.Player.IsLandMineUnlocked = true;
          stateManager.GameState.Player.StartingHealth = 0;
          StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
          GameManager.Notifications.PostNotification(this, "LevelLoaded");
     }

     public void OpenLevelFeedback()
     {
          Application.OpenURL("https://www.surveymonkey.com/r/2V36DGV");
     }

     public void OpenGameFeedback()
     {
          Application.OpenURL("https://www.surveymonkey.com/r/275DBHM");
     }
}
