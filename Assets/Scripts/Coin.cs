using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    public int value = 25;

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<Player>() == null) return;

        GameManager.instance.AddPoints(value);
        gameObject.SetActive(false);
    }

    public void setValue(int newValue) {
        value = newValue;
    }
}
