using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopCanvasController : MonoBehaviour
{
     Button exitShopButton;
     // Use this for initialization
     void Start()
     {
          exitShopButton = GameObject.Find("EscapeButton").GetComponent<Button>();
          exitShopButton.onClick.AddListener(delegate { ExitShop(); });
     }

     public void ExitShop()
     {
          GameManager.Notifications.PostNotification(this, "OnPlayerExitShop");
     }


}
