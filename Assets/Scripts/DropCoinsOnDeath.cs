using UnityEngine;
using System.Collections;

public class DropCoinsOnDeath : MonoBehaviour {

    public Coin coin;

    public void DropCoins(int amount) {
        coin.setValue(amount);
        Instantiate(coin, transform.position, transform.rotation);
    }

}
