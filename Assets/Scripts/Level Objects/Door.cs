using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    Sprite opened;
    SpriteRenderer sr;
    private BoxCollider2D collider;
	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
	}

    // Open the door by disabling the collider and changin the sprite 
    public void OpenDoor() {
        collider.enabled = false;
        if (opened != null) {
            sr.sprite = opened;
        }else{
            sr.enabled = false;
        }
    }
}
