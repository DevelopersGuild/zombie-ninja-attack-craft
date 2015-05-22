using UnityEngine;
using System.Collections;

public class Coin : Pickup {
    public Sprite crystal;
    public Sprite gold;
    public Sprite emerald;
    public Sprite ruby;
    public Sprite diamond;

    public override void Start(){
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (ValueOfPickup <= 25) {
            sr.sprite = crystal;
        }
        else if (ValueOfPickup > 25 && ValueOfPickup <= 50) {
            sr.sprite = gold;
        }
        else if (ValueOfPickup > 50 && ValueOfPickup <= 100) {
            sr.sprite = emerald;
        }
        else if (ValueOfPickup > 100 && ValueOfPickup <= 200) {
            sr.sprite = ruby;
        }
        else {
            sr.sprite = diamond;
        }
    }


    public override void AddItemToInventory(Collider2D player, int value) {
        GameManager.AddCoins(value);
    }


    public void setValue(int newValue) {
        ValueOfPickup = newValue;
    }
}
