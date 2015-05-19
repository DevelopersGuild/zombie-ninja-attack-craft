using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEndController : MonoBehaviour
{
     //GUI Text
     public Text ScoreGUI = null;
     public Text CoinsGUI = null;
     public Text TimeGUI = null;
     // Use this for initialization

     public void Start()
     {

          GameManager.Notifications.AddListener(this, "EndOfLevelReached");
     }

     public void EndOfLevelReached()
     {
          ScoreGUI.text = GameManager.getScore().ToString();
          CoinsGUI.text = GameManager.getCoins().ToString();
          TimeGUI.text = GameManager.getTime().ToString();
     }

}
