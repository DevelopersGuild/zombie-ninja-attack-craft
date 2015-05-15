using UnityEngine;
using System.Collections;

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

     public static int points;

     // Use this for initialization
     void Start()
     {
          currentLevel = Application.loadedLevel;
          points = 0;
          LoadGameData();

     }

     public static void AddPoints(int added)
     {
          points += added;
     }

     public static void SubtractPoints(int subtract)
     {
          points -= subtract;
     }

     public void Reset()
     {
          points = 0;
     }

     public static void setPoints(int pointsSet)
     {
          points = pointsSet;
     }

     public static int getPoints()
     {
          return points;
     }

     public void LoadGameData()
     {
          StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");
     }

     public static void LevelComplete()
     {
          bool isActive = StateManager.isActiveAndEnabled;
          if(stateManager.GameState.GameLevels.Count > currentLevel)
          {
               if(stateManager.GameState.GameLevels[currentLevel - 1].Score < points)
               {
                    stateManager.GameState.GameLevels[currentLevel - 1].Score = points;
               }

          }
          else
          {
               LoadAndSaveManager.GameStateData.GameLevelData newLevel = new LoadAndSaveManager.GameStateData.GameLevelData();
               newLevel.LevelUnlocked = true;
               newLevel.Score = points;
               stateManager.GameState.GameLevels.Add(newLevel);
          }
     }

     public void SaveGame()
     {
          LevelComplete();
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
