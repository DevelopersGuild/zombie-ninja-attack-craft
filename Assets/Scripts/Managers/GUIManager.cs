/*
 * Controls the visiablility of the different Canvases in unity
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour
{
     public Canvas MainTitleMenu = null;
     public Canvas restartCanvas = null;
     public Canvas StoreCanvas = null;
     public Canvas LoadLevelCanvas = null;
     public Button restartButton = null;
     public Button TitleScreenButton = null;


     void Start()
     {
          GameManager.Notifications.AddListener(this, "OnPlayerDeath");
          GameManager.Notifications.AddListener(this, "OnPlayerEnterShop");
          GameManager.Notifications.AddListener(this, "OnPlayerExitShop");

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
               if(restartButton != null)
               {
                    restartButton.interactable = false;
               }
               if (restartButton != null)
               {
                    TitleScreenButton.interactable = false;
               }

               restartCanvas.enabled = false;


          }

          if (LoadLevelCanvas != null)
          {
               LoadLevelCanvas.enabled = false;
          }

     }

     public void OnPlayerDeath()
     {
          if (restartCanvas != null)
          {
               restartCanvas.enabled = true;
               if (restartButton != null)
               {
                    restartButton.interactable = true;
               }
               if (restartButton != null)
               {
                    TitleScreenButton.interactable = true;
               }
               Cursor.visible = true;
          }
     }

     public void OnPlayerEnterShop()
     {
          if (StoreCanvas != null)
          {
               StoreCanvas.enabled = true;
               Cursor.visible = true;
          }
     }

     public void OnPlayerExitShop()
     {
          if (StoreCanvas != null)
          {
               StoreCanvas.enabled = false;
               Cursor.visible = false;
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
}
