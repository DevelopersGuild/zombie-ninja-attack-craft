/*
 * Controls the GUI for the Shop
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopGUI : MonoBehaviour
{
     //GUI Text 
     public Text ItemNameGUI;
     public Text NumberAvailable;
     public Text CostOfItem;
     public Text ResultMessage = null;


     public string ItemName = null;
     public Shop Store = null;


     void Start()
     {
          ItemNameGUI.text = ItemName;
          CostOfItem.text = Store.GetItemPrice(ItemName).ToString();
          NumberAvailable.text = Store.GetItemQuantity(ItemName).ToString();

          ResultMessage.text = "";
     }

     public void AttemptToBuyItem(string ItemName)
     {
          string ResultOfTransaction;
          ResultOfTransaction = Store.BuyItem(ItemName);
          ResultMessage.text = ResultOfTransaction;
          NumberAvailable.text = Store.GetItemQuantity(ItemName).ToString();
     }






}
