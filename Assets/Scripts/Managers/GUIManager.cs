/*
 * Controls the visiablility of the different Canvases in unity
 */

using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
     public Canvas MainTitleMenu = null;
     public Canvas restartCanvas = null;
     public Canvas StoreCanvas = null;
     public Canvas LoadLevelCanvas = null;
     public Canvas EndOfLevelCanvas = null;
     public Canvas SettingsCanvas = null;

     private bool isInSettings = false; 

     void Start()
     {
          GameManager.Notifications.AddListener(this, "OnPlayerDeath");
          GameManager.Notifications.AddListener(this, "OnPlayerEnterShop");
          GameManager.Notifications.AddListener(this, "OnPlayerExitShop");
          GameManager.Notifications.AddListener(this, "EndOfLevelReached");

          if (Application.loadedLevelName != "titleScreen")
          {
               Cursor.visible = false;
          }

          if (StoreCanvas != null)
          {
               StoreCanvas.enabled = false;
          }

          if (restartCanvas != null)
          {
               restartCanvas.enabled = false;
          }

          if (LoadLevelCanvas != null)
          {
               LoadLevelCanvas.enabled = false;
          }

          if (EndOfLevelCanvas != null)
          {
               EndOfLevelCanvas.enabled = false;
          }
          
          if (SettingsCanvas != null)
          {
               SettingsCanvas.enabled = false;
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
                         Cursor.visible = true;
                         GameManager.Instance.PauseGame();
                    }
                    else
                    {
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
               Cursor.visible = true;
          }
     }

     public void OnPlayerEnterShop()
     {
          if (StoreCanvas != null)
          {
               GameManager.Notifications.PostNotification(this, "PlayerInMenu");
               StoreCanvas.enabled = true;
               Cursor.visible = true;
               GameManager.Instance.PauseGame();
          }
     }

     public void OnPlayerExitShop()
     {
          if (StoreCanvas != null)
          {
               GameManager.Notifications.PostNotification(this, "PlayerExitMenu");
               StoreCanvas.enabled = false;
               Cursor.visible = false;
               GameManager.Instance.UnpauseGame();
          }
     }

     public void EndOfLevelReached()
     {
          if (EndOfLevelCanvas != null)
          {
               GameManager.Notifications.PostNotification(this, "PlayerInMenu");
               EndOfLevelCanvas.enabled = true;
               Cursor.visible = true;
               GameManager.Instance.PauseGame();
          }
     }

     public void ShowLoadLevel()
     {
          MainTitleMenu.enabled = false;
          LoadLevelCanvas.enabled = true;
     }

     public void ShowTitleScreen()
     {
          MainTitleMenu.enabled = true;
          LoadLevelCanvas.enabled = false;
     }

     public void ShowSettingScreen()
     {
          SettingsCanvas.enabled = true;
          if(restartCanvas != null)
          {
               restartCanvas.enabled = false;
          }

          if(MainTitleMenu != null)
          {
               MainTitleMenu.enabled = false;
          }
          isInSettings = true;
     }

     public void HideSettingScreen()
     {
          SettingsCanvas.enabled = false;
          if (restartCanvas != null)
          {
               restartCanvas.enabled = true;
          }

          if (MainTitleMenu != null)
          {
               MainTitleMenu.enabled = true;
          }
          isInSettings = false;
     }

     private void OnGUI()
     {

     }

}

