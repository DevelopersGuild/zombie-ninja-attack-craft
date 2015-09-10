/*
 * Controls the visiablility of the different Canvases in unity
 */

using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
     private Canvas mainTitleMenu = null;
     private Canvas loadLevelCanvas = null;
     private Canvas restartCanvas = null;
     private Canvas storeCanvas = null;
     private Canvas endOfLevelCanvas = null;
     private Canvas settingsCanvas = null;

     private bool isInSettings = false; 

     void Start()
     {
          GameManager.Notifications.AddListener(this, "OnPlayerDeath");
          GameManager.Notifications.AddListener(this, "OnPlayerEnterShop");
          GameManager.Notifications.AddListener(this, "OnPlayerExitShop");
          GameManager.Notifications.AddListener(this, "EndOfLevelReached");

          GameObject temp = GameObject.Find("TitleScreenCanvas");
          if(temp != null)
          {
               mainTitleMenu = temp.GetComponent<Canvas>();
               mainTitleMenu.GetComponent<KeyBoardControl>().SetIsInMenu(true);
          }
          temp = GameObject.Find("LoadLevelCanvas");
          if (temp != null)
          {
               loadLevelCanvas = temp.GetComponent<Canvas>();
          }
          temp = GameObject.Find("RestartCanvas");
          if (temp != null)
          {
               restartCanvas = temp.GetComponent<Canvas>();
          }
          temp = GameObject.Find("ShopCanvas");
          if (temp != null)
          {
               storeCanvas = temp.GetComponent<Canvas>();
          }
          temp = GameObject.Find("EndOfLevelCanvas");
          if (temp != null)
          {
               endOfLevelCanvas = temp.GetComponent<Canvas>();
          }
          temp = GameObject.Find("SettingsCanvas");
          if (temp != null)
          {
               settingsCanvas = temp.GetComponent<Canvas>();
          }


          if (Application.loadedLevelName != "titleScreen")
          {
               Cursor.visible = false;
          }

          if (storeCanvas != null)
          {
               storeCanvas.enabled = false;
          }

          if (restartCanvas != null)
          {
               restartCanvas.enabled = false;
          }

          if (loadLevelCanvas != null)
          {
               loadLevelCanvas.enabled = false;
          }

          if (endOfLevelCanvas != null)
          {
               endOfLevelCanvas.enabled = false;
          }
          
          if (settingsCanvas != null)
          {
               settingsCanvas.enabled = false;
          }

          GUIStyle myStyle = new GUIStyle();
     }

     void Update()
     {
          if(Input.GetButtonDown("Cancel") && isInSettings == false)
          {
               if(restartCanvas != null)
               {
                    if(restartCanvas.enabled == false)
                    {
                         restartCanvas.enabled = true;
                         SetCanvasKeyBoardController(restartCanvas, true);
                         Cursor.visible = true;
                         GameManager.Instance.PauseGame();
                    }
                    else
                    {
                         SetCanvasKeyBoardController(restartCanvas, false);
                         restartCanvas.enabled = false;
                         Cursor.visible = false;
                         GameManager.Instance.UnpauseGame();
                    }
               }
          }
     }

     public void OnPlayerDeath()
     {
          if (restartCanvas != null)
          {
               restartCanvas.enabled = true;
               GameManager.incrementDeaths();
               SetCanvasKeyBoardController(restartCanvas, true);
               Cursor.visible = true;
          }
     }

     public void OnPlayerEnterShop()
     {
          if (storeCanvas != null)
          {
               GameManager.Notifications.PostNotification(this, "PlayerInMenu");
               storeCanvas.enabled = true;
               SetCanvasKeyBoardController(storeCanvas, true);
               Cursor.visible = true;
               GameManager.Instance.PauseGame();
          }
     }

     public void OnPlayerExitShop()
     {
          if (storeCanvas != null)
          {
               GameManager.Notifications.PostNotification(this, "PlayerExitMenu");
               SetCanvasKeyBoardController(storeCanvas, false);
               storeCanvas.enabled = false;
               Cursor.visible = false;
               GameManager.Instance.UnpauseGame();
          }
     }

     public void EndOfLevelReached()
     {
          if (endOfLevelCanvas != null)
          {
               SetCanvasKeyBoardController(endOfLevelCanvas, true);
               GameManager.Notifications.PostNotification(this, "PlayerInMenu");
               endOfLevelCanvas.enabled = true;
               Cursor.visible = true;
               GameManager.Instance.PauseGame();
          }
     }

     public void ShowLoadLevel()
     {
          SetCanvasKeyBoardController(mainTitleMenu, false);
          mainTitleMenu.enabled = false;
          loadLevelCanvas.enabled = true;
          SetCanvasKeyBoardController(loadLevelCanvas, true);
     }

     public void ShowTitleScreen()
     {
          SetCanvasKeyBoardController(loadLevelCanvas, false);
          mainTitleMenu.enabled = true;
          loadLevelCanvas.enabled = false;
          SetCanvasKeyBoardController(mainTitleMenu, true);
     }

     public void ShowSettingScreen()
     {
          settingsCanvas.enabled = true;
          if(restartCanvas != null)
          {
               restartCanvas.enabled = false;
          }

          if(mainTitleMenu != null)
          {
               mainTitleMenu.enabled = false;
          }
          isInSettings = true;
     }

     public void HideSettingScreen()
     {
          settingsCanvas.enabled = false;
          if (restartCanvas != null)
          {
               restartCanvas.enabled = true;
          }

          if (mainTitleMenu != null)
          {
               mainTitleMenu.enabled = true;
          }
          isInSettings = false;
     }

     private void SetCanvasKeyBoardController(Canvas canvas, bool value)
     {
          canvas.GetComponent<KeyBoardControl>().SetIsInMenu(value);
     }

     private void OnGUI()
     {

     }

}

