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
          if (ValueOfPickup <= 25)
          {
               sr.sprite = crystal;
          }
          else if (ValueOfPickup > 25 && ValueOfPickup <= 100)
          {
               sr.sprite = gold;
          }
          else if (ValueOfPickup > 100 && ValueOfPickup <= 175)
          {
               sr.sprite = emerald;
          }
          else if (ValueOfPickup > 175 && ValueOfPickup <= 250)
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
          ValueOfPickup = newValue;
     }
}
