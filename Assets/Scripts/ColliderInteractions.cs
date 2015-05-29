using UnityEngine;
using System.Collections;

public class ColliderInteractions : MonoBehaviour {

	// Use this for initialization
    void Start() {

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Chest>()) {
            Debug.Log("CHEST!");
            other.GetComponent<Chest>().OpenChest();
        }
    }
}
