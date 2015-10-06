using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RestartCanvasController : MonoBehaviour
{
     private Button restartLevelButton;
     private Button titleScreenButton;
     private Button settingsButton;
     private GUIManager guiManager;
     // Use this for initialization
     void Start()
     {
          restartLevelButton = GameObject.Find("RestartButton").GetComponent<Button>();
          restartLevelButton.onClick.AddListener( delegate { RestartLevel(); });

          titleScreenButton = GameObject.Find("TitleScreenButtonR").GetComponent<Button>();
          titleScreenButton.onClick.AddListener(delegate { TitleScreen(); });

          settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
          settingsButton.onClick.AddListener(delegate { ShowSettingsCanvas(); });

          guiManager = GameObject.Find("GUIEventSystem").GetComponent<GUIManager>();
     }

     public void RestartLevel()
     {
          int what = GameManager.deaths;
          GameManager.Instance.SwitchLevel(GameManager.CurrentLevel);
     }

     public void TitleScreen()
     {
          GameManager.deaths = 0;
          GameManager.Instance.SwitchLevel(0);
     }

     public void ShowSettingsCanvas()
     {
          guiManager.ShowSettingScreen();
     }

}
