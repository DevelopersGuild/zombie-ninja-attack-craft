using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NotificationManager))]
public class GameManager : MonoBehaviour
{


     public static int points;
     
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
     void Awake()
     {
          if((instance) && (instance.GetInstanceID() != GetInstanceID()))
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


     // Use this for initialization
     void Start()
     {
          points = 0;
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

     public void SwitchLevel(int level)
     {
          Application.LoadLevel(level);
     }
}
