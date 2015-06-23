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

          GUIStyle myStyle = new GUIStyle();
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
          }
     }

     public void OnPlayerExitShop()
     {
          if (StoreCanvas != null)
          {
               GameManager.Notifications.PostNotification(this, "PlayerExitMenu");
               StoreCanvas.enabled = false;
               Cursor.visible = false;
          }
     }

     public void EndOfLevelReached()
     {
          if (EndOfLevelCanvas != null)
          {
               GameManager.Notifications.PostNotification(this, "PlayerInMenu");
               EndOfLevelCanvas.enabled = true;
               Cursor.visible = true;
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

     private void OnGUI()
     {

     }

}

