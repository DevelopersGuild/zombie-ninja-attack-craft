using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
     private Dictionary<string, InventoryItem> Inventory = new Dictionary<string, InventoryItem>();
     private Pickup boughtItem;
     Collider2D player;
     //Projectile
     public int QuantityOfArrows;
     public int PriceOfArrows;
     public int QuantityArrowsSoldIn;

     //Bomb
     public int QuantityOfBombs;
     public int PriceOfBombs;
     public int QuantityBombsSoldIn;

     //Health
     public int QuantityOfHealth;
     public int PriceOfHealth;
     public int QuantityHealthSoldIn;

     void Start()
     {
          GameObject temp = new GameObject();
          temp.AddComponent<AmmoPickup>();
          Inventory.Add("Arrows", new InventoryItem());
          Inventory["Arrows"].TypeOfItem = temp.GetComponent<Pickup>();
          Inventory["Arrows"].QuantityOfItem = QuantityOfArrows;
          Inventory["Arrows"].PriceOfItem = PriceOfArrows;
          Inventory["Arrows"].QuantitySoldIn = QuantityArrowsSoldIn;

          temp = new GameObject();
          temp.AddComponent<BombAmmoPickup>();
          Inventory.Add("Bomb", new InventoryItem());
          Inventory["Bomb"].TypeOfItem = temp.GetComponent<Pickup>();
          Inventory["Bomb"].QuantityOfItem = QuantityOfBombs;
          Inventory["Bomb"].PriceOfItem = PriceOfBombs;
          Inventory["Bomb"].QuantitySoldIn = QuantityBombsSoldIn;

          temp = new GameObject();
          temp.AddComponent<HealthPickup>();
          Inventory.Add("Health", new InventoryItem());
          Inventory["Health"].TypeOfItem = temp.GetComponent<Pickup>();
          Inventory["Health"].QuantityOfItem = QuantityOfHealth;
          Inventory["Health"].PriceOfItem = PriceOfHealth;
          Inventory["Health"].QuantitySoldIn = QuantityHealthSoldIn;
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

     public string BuyItem(string desiredItem, int desiredQuantity = 1)
     {
          desiredQuantity = Inventory[desiredItem].QuantitySoldIn;
          string displayMessage = "You have purchased this item";
          if (Inventory[desiredItem].QuantityOfItem >= desiredQuantity)
          {
               if (GameManager.getCoins() >= Inventory[desiredItem].PriceOfItem)
               {
                    SpawnItem(desiredItem, desiredQuantity);
                    GameManager.SubtractCoins(Inventory[desiredItem].PriceOfItem);
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
