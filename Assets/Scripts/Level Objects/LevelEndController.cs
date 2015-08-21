using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEndController : MonoBehaviour
{
     //GUI Text
     public Text ScoreGUI = null;
     public Text CoinsGUI = null;
     public Text TimeGUI = null;

     //GUI Button
     public Button ReplayButton = null;
     public Button NextLevelButton = null;
     // Use this for initialization


     public void Start()
     {
          NextLevelButton.enabled = true;
          GameManager.Notifications.AddListener(this, "ScoreReadyToDisplay");
     }

     public void ScoreReadyToDisplay()
     {
          int minutes = 0;
          int seconds = 0;
          ScoreGUI.text = GameManager.getScore().ToString();
          CoinsGUI.text = GameManager.getCoins().ToString();
          float time = GameManager.getTime();
          if (time > 60)
          {
               minutes = (int)time / 60;
               seconds = (int)time % 60;
          }
          else
          {
               seconds = (int)time;
          }
          TimeGUI.text = minutes + " Minutes and " + seconds + " Seconds";

          if (GameManager.getIsLevelComplete() == true)
          {
               NextLevelButton.enabled = true;
          }

     }

}
