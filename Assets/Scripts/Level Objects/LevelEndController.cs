using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEndController : MonoBehaviour
{
     //GUI Text
     public Text ScoreGUI = null;
     public Text CoinsGUI = null;
     public Text TimeGUI = null;
     public Text DeathsGUI = null;
     public Text EnemyKillsGUI = null;
     public Text HelpMessageGUI = null;

     //GUI Button
     public Button ReplayButton = null;
     public Button NextLevelButton = null;
     public Button GameFeedbackButton = null;
     // Use this for initialization


     public void Start()
     {
          NextLevelButton.enabled = true;
          GameFeedbackButton.gameObject.SetActive(false);
          GameManager.Notifications.AddListener(this, "ScoreReadyToDisplay");
     }

     public void ScoreReadyToDisplay()
     {
          int minutes = 0;
          int seconds = 0;
          ScoreGUI.text = GameManager.getScore().ToString();
          CoinsGUI.text = GameManager.getCoins().ToString();
          EnemyKillsGUI.text = GameManager.getKills().ToString();
          DeathsGUI.text = GameManager.getDeaths().ToString();

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

          if(GameManager.CurrentLevel <3)
          {
               HelpMessageGUI.text = "Please send your score data to our servers by clicking the send score button. Any feedback/bug reports are also appreciated. Thanks for testing!";
          }
          else
          {
               HelpMessageGUI.text = "Hey! You've played a good amount. Care to give us general thoughts on the game so far?";
               GameFeedbackButton.gameObject.SetActive(true);
          }

          if (GameManager.getIsLevelComplete() == true)
          {
               NextLevelButton.enabled = true;
          }

     }

}
