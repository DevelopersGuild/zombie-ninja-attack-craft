using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomUnlock : MonoBehaviour {

    public List<Door> doors = new List<Door>();
    public List<GameObject> enemies = new List<GameObject>();
    private int enemyCount;
    private bool isCleared;

    private bool isActive;

	// Use this for initialization
	void Start () {
        enemyCount = 0;
        isCleared = false;
        isActive = false;
        foreach (Door door in doors) {
            door.OpenDoor();
        }
        foreach (GameObject enemy in enemies) {
            enemy.SetActive(false);
            enemyCount++;
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            ActivateRoom();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isActive) {
            Debug.Log(enemies.Count);
            Debug.Log(isCleared);
            isCleared = true;
            foreach (GameObject enemy in enemies) {
                if (enemy != null) {
                    isCleared = false;
                }
            }
            if (isCleared) {
                closeRoom();
            }


        }
	}

    void ActivateRoom() {
        foreach (Door door in doors) {
            door.CloseDoor();
        }
        foreach (GameObject enemy in enemies) {
            enemy.SetActive(true);
        }
        isActive = true;
    }

    void closeRoom() {
        foreach (Door door in doors) {
            door.OpenDoor();
        }
        Destroy(gameObject);
    }
}
