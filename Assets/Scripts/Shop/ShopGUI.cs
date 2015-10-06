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
     private Shop store = null;


     void Start()
     {
          store = GameObject.Find("StoreTrigger").GetComponent<Shop>();
          ItemNameGUI.text = ItemName;
          CostOfItem.text = store.GetItemPrice(ItemName).ToString();
          NumberAvailable.text = store.GetItemQuantity(ItemName).ToString();

          ResultMessage.text = "";

     }

     public void AttemptToBuyItem(string ItemName)
     {
          string ResultOfTransaction;
          ResultOfTransaction = store.BuyItem(ItemName);
          ResultMessage.text = ResultOfTransaction;
          NumberAvailable.text = store.GetItemQuantity(ItemName).ToString();
     }






}
