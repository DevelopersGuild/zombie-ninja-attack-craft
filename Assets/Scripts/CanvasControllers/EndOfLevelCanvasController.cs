using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndOfLevelCanvasController : MonoBehaviour
{
     private Button replayLevelButton;
     private Button titleScreenButton;
     private Button nextLevelButton;
     private Button sendScoreButton;
     public Button gameFeedBackButton;
     private bool hasSentFeedback;
     // Use this for initialization
     void Start()
     {
          replayLevelButton = GameObject.Find("ReplayLevelButton").GetComponent<Button>();
          replayLevelButton.onClick.AddListener(delegate { OnClickOfReplayLevel(); });

          titleScreenButton = GameObject.Find("TitleScreenButtonE").GetComponent<Button>();
          titleScreenButton.onClick.AddListener(delegate { TitleScreen(); });

          nextLevelButton = GameObject.Find("NextLevelButton").GetComponent<Button>();
          nextLevelButton.onClick.AddListener(delegate { NextLevel(); });

          sendScoreButton = GameObject.Find("SendScoreButton").GetComponent<Button>();
          sendScoreButton.onClick.AddListener(delegate { SendScore(); });

     }

     public void OnClickOfReplayLevel()
     {
          GameManager.deaths = 0;
          GameManager.Instance.SwitchLevel(GameManager.CurrentLevel);
     }

     public void TitleScreen()
     {
          GameManager.deaths = 0;
          GameManager.Instance.SwitchLevel(0);
     }

     public void NextLevel()
     {
          GameManager.deaths = 0;
          GameManager.Instance.SwitchLevel(GameManager.CurrentLevel+1);
     }
     public void SendScore()
     {
          if(!hasSentFeedback){
               ScoreForm form = GetComponent<ScoreForm>();
               form.StartCoroutine(form.UploadScores());
               sendScoreButton.enabled = false;
               sendScoreButton.GetComponentInChildren<Text>().text = "Thanks!";
               hasSentFeedback = true;
          }
     }


}
