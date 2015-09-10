using UnityEngine;
using System.Collections;

public class ScoreForm : MonoBehaviour
{
     string highscore_url = "http://pariahsoft.com:8080";
     string playName = "Player 1";
     int currentLevel;
     int score;
     int coins;
     int deaths;
     int kills;
     string timeFormatted;
     float time;
     int minutes, seconds;

     // Use this for initialization
     public IEnumerator UploadScores()
     {
          // Create a form object for sending high score data to the server
          WWWForm form = new WWWForm();

          currentLevel = GameManager.CurrentLevel;
          score = GameManager.getScore();
          coins = GameManager.getCoins();
          timeFormatted = GameManager.getTimeFormatted();
          deaths = GameManager.getDeaths();
          kills = GameManager.getKills();

          // Assuming the perl script manages high scores for different games
          form.AddField("CurrentLevel", currentLevel);
          form.AddField("Time", timeFormatted);
          form.AddField("Deaths", deaths);
          form.AddField("Kills", kills);
          form.AddField("Score", score);
          form.AddField("Coins", coins);

          // Create a download object
          WWW download = new WWW(highscore_url, form);

          Debug.Log("Created!");

          // Wait until the download is done
          yield return download;

          Debug.Log("Finished!");

          if (!string.IsNullOrEmpty(download.error))
          {
               print("Error downloading: " + download.error);
          }
          else
          {
               // show the highscores
               Debug.Log(download.text);
          }
     }
}