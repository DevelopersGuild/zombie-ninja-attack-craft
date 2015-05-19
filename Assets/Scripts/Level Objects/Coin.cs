using UnityEngine;
using System.Collections;

public class Coin : Pickup
{
     

     public override void AddItemToInventory(Collider2D player, int value)
     {
          GameManager.AddCoins(value);
     }
     

     public void setValue(int newValue)
     {
          ValueOfPickup = newValue;
     }
}
