using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    public int value = 25;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>() == null) return;

       GameManager.AddCoins(value);
        gameObject.SetActive(false);
    }

    public void setValue(int newValue) {
        value = newValue;
    }
}
