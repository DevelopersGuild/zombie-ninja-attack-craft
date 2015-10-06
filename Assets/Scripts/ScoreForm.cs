using UnityEngine;
using System.Collections;

public class ScoreForm : MonoBehaviour
{

     public Player player;

     string highscore_url = "http://107.191.105.122:8080/";
     string playName = "Player 1";
     int currentLevel;
     int score;
     int coins;
     int deaths;
     int kills;
     int ammo;
     int grenades;
     float healthLeft;
     string timeFormatted;
     float time;
     int minutes, seconds;

     // Use this for initialization
     public IEnumerator UploadScores()
     {
          player = FindObjectOfType<Player>();
          AttackController playerAttack = player.GetComponent<AttackController>();
          // Create a form object for sending high score data to the server
          WWWForm form = new WWWForm();

          currentLevel = GameManager.CurrentLevel;
          score = GameManager.getScore();
          coins = GameManager.getCoins();
          timeFormatted = GameManager.getTimeFormatted();
          deaths = GameManager.getDeaths();
          kills = GameManager.getKills();
          healthLeft = player.GetComponent<Health>().currentHp();
          ammo = playerAttack.Ammo;
          grenades = playerAttack.Grenades;


          // Assuming the perl script manages high scores for different games
          form.AddField("CurrentLevel", currentLevel);
          form.AddField("Time", timeFormatted);
          form.AddField("Deaths", deaths);
          form.AddField("Kills", kills);
          form.AddField("Score", score);
          form.AddField("Coins", coins);
          form.AddField("HealthLeft", healthLeft.ToString());
          form.AddField("Ammo", ammo);
          form.AddField("Grenades", grenades);

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