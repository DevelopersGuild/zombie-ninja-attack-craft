using UnityEngine;
using System.Collections;

public class Coin : Pickup
{
     public Sprite crystal;
     public Sprite gold;
     public Sprite emerald;
     public Sprite ruby;
     public Sprite diamond;
     System.Random rnd;

     public override void Start()
     {
          // Set coisn value to be random
          ValueOfPickup = Random.Range(MinimumValueRange, MaximumValueRange);

          // Set the sprite according to value
          SpriteRenderer sr = GetComponent<SpriteRenderer>();
          if (ValueOfPickup <= 40)
          {
               sr.sprite = crystal;
          }
          else if (ValueOfPickup > 40 && ValueOfPickup <= 150)
          {
               sr.sprite = gold;
          }
          else if (ValueOfPickup > 150 && ValueOfPickup <= 250)
          {
               sr.sprite = emerald;
          }
          else if (ValueOfPickup > 250 && ValueOfPickup <= 400)
          {
               sr.sprite = ruby;
          }
          else
          {
               sr.sprite = diamond;
          }
     }


     public override void AddItemToInventory(Collider2D player, int value)
     {
          GameManager.AddCoins(value);
     }

     public override void sendPickupMessage()
     {
          GameManager.Notifications.PostNotification(this, "CoinPickedUp");
     }

     public void setValue(int newValue)
     {
          MinimumValueRange = newValue;
          MaximumValueRange = newValue;
     }
}
