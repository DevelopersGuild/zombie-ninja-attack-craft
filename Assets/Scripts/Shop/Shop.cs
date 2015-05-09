using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
     private Dictionary<string, InventoryItem> Inventory = new Dictionary<string, InventoryItem>();
     private Pickup boughtItem;
     Collider2D player;

     void Start()
     {
          GameObject temp = new GameObject();
          temp.AddComponent<AmmoPickup>();
          Inventory.Add("Arrows", new InventoryItem());
          Inventory["Arrows"].TypeOfItem = temp.GetComponent<Pickup>();
          Inventory["Arrows"].QuantityOfItem = 5;
          Inventory["Arrows"].PriceOfItem = 1;
          /*
          Inventory.Add("Health", new InventoryItem());
          Inventory["Health"].TypeOfItem = new HealthPickup();
          Inventory["Health"].QuantityOfItem = 20;
          Inventory["Health"].PriceOfItem = 2;
           */
     }

     void BuildInventory()
     {

     }

     public int GetItemPrice(string Name)
     {
          return Inventory[Name].PriceOfItem;
     }

     public int GetItemQuantity(string Name)
     {
          return Inventory[Name].QuantityOfItem;
     }

     public string BuyItem(string desiredItem, int desiredQuantity)
     {
          string displayMessage = "You have purchased this item";
          if (Inventory[desiredItem].QuantityOfItem >= desiredQuantity)
          {
               if(GameManager.getPoints() >= (Inventory[desiredItem].PriceOfItem * desiredQuantity))
               {
                    SpawnItem(desiredItem, desiredQuantity);
                    GameManager.SubtractPoints(Inventory[desiredItem].PriceOfItem * desiredQuantity);
                    Inventory[desiredItem].QuantityOfItem -= desiredQuantity;

               }
               else
               {
                    displayMessage = "You do not have enough money";
               }
          }
          else
          {
               displayMessage = "Not enough of that item";
          }
          return displayMessage;
     }

     public void SpawnItem(string pickupToSpawn, int quantity)
     {
          Inventory[pickupToSpawn].TypeOfItem.AddItemToInventory(player, quantity);
     }

     //Debugging----------------------------------------------------------------------
     public void OnTriggerEnter2D(Collider2D other)
     {
          if(other.gameObject.tag == "Player")
          {
               player = other;
               GameManager.Notifications.PostNotification(this, "OnPlayerEnterShop");
          }

     }

     public void OnTriggerExit2D(Collider2D other)
     {
          if (other.gameObject.tag == "Player")
          {
               GameManager.Notifications.PostNotification(this, "OnPlayerExitShop");
          }
     }



     

}
