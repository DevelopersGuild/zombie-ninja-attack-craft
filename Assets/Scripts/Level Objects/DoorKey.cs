using UnityEngine;
using System.Collections;

public class DoorKey : MonoBehaviour {

    public Door door;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            door.OpenDoor();
            //PLAY SOUND
            Destroy(gameObject);
        }
    }
}
