/*
 * Controls the visiablility of the different Canvases in unity
 */

using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
     public Canvas restartCanvas = null;
     public Canvas StoreCanvas = null;

     void Start()
     {
          GameManager.Notifications.AddListener(this, "OnPlayerDeath");
          GameManager.Notifications.AddListener(this, "OnPlayerEnterShop");
          GameManager.Notifications.AddListener(this, "OnPlayerExitShop");
          restartCanvas.enabled = false;
          StoreCanvas.enabled = false;
     }

     public void OnPlayerDeath()
     {
          restartCanvas.enabled = true;
     }

     public void OnPlayerEnterShop()
     {
          StoreCanvas.enabled = true;
     }

     public void OnPlayerExitShop()
     {
          StoreCanvas.enabled = false;
     }
}
