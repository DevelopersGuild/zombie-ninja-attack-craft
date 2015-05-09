/*
 * Controls the GUI for the Shop
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopGUI : MonoBehaviour
{
     public Text HowManyThePlayerWants = null;
     public string ItemName = null;
     public Text ItemNameGUI;

     public Shop Store = null;

     private int AmountToBuy = 1;


     void Start()
     {
          Store.GetItemPrice(ItemName);
          HowManyThePlayerWants.text = "1";
     }

     public void AddToAmountToBuy(int number)
     {
          
          AmountToBuy += number;
          if (AmountToBuy < 1 )
          {
               AmountToBuy = 1;
          }
          HowManyThePlayerWants.text = AmountToBuy.ToString();
     }

     public void AttemptToBuyItem(string ItemName)
     {
          string Result;
          Result = Store.BuyItem(ItemName, AmountToBuy);
     }

     public void NotEnoughMoney()
     {

     }

     public void NotEnoughItem()
     {

     }





}
