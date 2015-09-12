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
          GameManager.Notifications.AddListener(this, "TurnOnEndOfLevelCanvas");

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
               storeCanvas.gameObject.SetActive(false);
          }

          if (restartCanvas != null)
          {
               restartCanvas.gameObject.SetActive(false);
          }

          if (loadLevelCanvas != null)
          {
               loadLevelCanvas.gameObject.SetActive(false);
          }

          if (endOfLevelCanvas != null)
          {
               endOfLevelCanvas.gameObject.SetActive(false);
          }
          
          if (settingsCanvas != null)
          {
               settingsCanvas.gameObject.SetActive(false);
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
                         restartCanvas.gameObject.SetActive(true);
                         SetCanvasKeyBoardController(restartCanvas, true);
                         Cursor.visible = true;
                         GameManager.Instance.PauseGame();
                    }
                    else
                    {
                         SetCanvasKeyBoardController(restartCanvas, false);
                         restartCanvas.gameObject.SetActive(false);
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
               restartCanvas.gameObject.SetActive(true);
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
               storeCanvas.gameObject.SetActive(true);
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
               storeCanvas.gameObject.SetActive(false);
               Cursor.visible = false;
               GameManager.Instance.UnpauseGame();
          }
     }

     public void TurnOnEndOfLevelCanvas()
     {
          if (endOfLevelCanvas != null)
          {
               GameManager.Notifications.PostNotification(this, "PlayerInMenu");
               endOfLevelCanvas.gameObject.SetActive(true);
               SetCanvasKeyBoardController(endOfLevelCanvas, true);
               Cursor.visible = true;
               GameManager.Instance.PauseGame();
          }
     }

     public void ShowLoadLevel()
     {
          SetCanvasKeyBoardController(mainTitleMenu, false);
          mainTitleMenu.gameObject.SetActive(false);
          loadLevelCanvas.gameObject.SetActive(true);
          SetCanvasKeyBoardController(loadLevelCanvas, true);
     }

     public void ShowTitleScreen()
     {
          SetCanvasKeyBoardController(loadLevelCanvas, false);
          mainTitleMenu.gameObject.SetActive(true);
          loadLevelCanvas.gameObject.SetActive(false);
          SetCanvasKeyBoardController(mainTitleMenu, true);
     }

     public void ShowSettingScreen()
     {
          settingsCanvas.gameObject.SetActive(true);
          if(restartCanvas != null)
          {
               restartCanvas.gameObject.SetActive(false);
          }

          if(mainTitleMenu != null)
          {
               mainTitleMenu.gameObject.SetActive(false);
          }
          isInSettings = true;
     }

     public void HideSettingScreen()
     {
          settingsCanvas.gameObject.SetActive(false);
          if (restartCanvas != null)
          {
               restartCanvas.gameObject.SetActive(true);
          }

          if (mainTitleMenu != null)
          {
               mainTitleMenu.gameObject.SetActive(true);
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

