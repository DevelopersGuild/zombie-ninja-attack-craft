/*
 * Controls the GUI for the Shop
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopGUI : MonoBehaviour
{
     //GUI Text 
     public Text HowManyThePlayerWants = null;
     public Text ItemNameGUI;
     public Text NumberAvailable;
     public Text CostOfItem;
     public Text ResultMessage = null;


     public string ItemName = null;
     public Shop Store = null;
     private int AmountToBuy = 1;


     void Start()
     {
          HowManyThePlayerWants.text = "1";
          ItemNameGUI.text = ItemName;
          NumberAvailable.text = Store.GetItemQuantity(ItemName).ToString();
          CostOfItem.text = Store.GetItemPrice(ItemName).ToString();
          ResultMessage.text = "";
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
          string ResultOfTransaction;
          ResultOfTransaction = Store.BuyItem(ItemName, AmountToBuy);
          ResultMessage.text = ResultOfTransaction;
          NumberAvailable.text = Store.GetItemQuantity(ItemName).ToString();
     }






}
