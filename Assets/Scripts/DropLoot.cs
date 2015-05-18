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
    public List<int> itemDropRates = new List<int>();
    private List<GameObject> itemDrop = new List<GameObject>();

    private int itemDropRate; // Probability that item will drop out of 10
    public int dropChance; // Probability that item will drop out of 100

    public void DropCoins(int amount) {
        coin.setValue(amount);
        Instantiate(coin, transform.position, transform.rotation);
    }

    public void DropItem() {
        // Check if an item will drop
        randomDropChance = Random.Range(0, 100);

        // If an item does drop, pick one randomly with the given drop chances
        if (randomDropChance <= dropChance) {
            itemDropRate = Random.Range(0, 10);

            for (int i = 0; i < items.Count; i++) {
                for (int j = 0; j < itemDropRates[i]; j++) {
                    itemDrop.Add(items[i]);
                }
            }
            //If it will drop an item, set the item it will drop to a random item that it can drop
            Debug.Log(itemDropRate);
            item = itemDrop[itemDropRate];

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
