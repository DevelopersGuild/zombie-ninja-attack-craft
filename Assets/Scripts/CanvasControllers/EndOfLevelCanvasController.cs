using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndOfLevelCanvasController : MonoBehaviour
{
     private Button replayLevelButton;
     private Button titleScreenButton;
     private Button nextLevelButton;
     // Use this for initialization
     void Start()
     {
          replayLevelButton = GameObject.Find("ReplayLevelButton").GetComponent<Button>();
          replayLevelButton.onClick.AddListener(delegate { OnClickOfReplayLevel(); });

          titleScreenButton = GameObject.Find("TitleScreenButtonE").GetComponent<Button>();
          titleScreenButton.onClick.AddListener(delegate { TitleScreen(); });

          nextLevelButton = GameObject.Find("NextLevelButton").GetComponent<Button>();
          nextLevelButton.onClick.AddListener(delegate { NextLevel(); });


     }

     public void OnClickOfReplayLevel()
     {
          GameManager.Instance.SwitchLevel(GameManager.CurrentLevel);
     }

     public void TitleScreen()
     {
          GameManager.Instance.SwitchLevel(0);
     }

     public void NextLevel()
     {
          GameManager.Instance.SwitchLevel(GameManager.CurrentLevel+1);
     }
}
