using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropLoot : MonoBehaviour {

    //public Coin coin;
    public int coinValue;
    public Coin coin;
    private GameObject item;
    private int randomDropChance;
    public List<GameObject> items = new List<GameObject>();
    public int dropChance;

    public void DropCoins(int amount) {
        coin.setValue(amount);
        Instantiate(coin, transform.position, transform.rotation);
    }

    public void DropItem() {
        //Fifty percent chance to drop nothing if its 0
        randomDropChance = Random.Range(dropChance, 2);
        if (randomDropChance >= 1) {
            //If it will drop an item, set the item it will drop to a random item that it can drop
            item = items[Random.Range(0, items.Count)];

            //If it was a coin, give it a value
            if (item.GetComponent<Coin>()) {
                coin = item.GetComponent<Coin>();
                coin.setValue(coinValue);
            }

            //Spawn the item
            if (item != null) {
                Instantiate(item, transform.position, transform.rotation);
            }
        }
    }

}
