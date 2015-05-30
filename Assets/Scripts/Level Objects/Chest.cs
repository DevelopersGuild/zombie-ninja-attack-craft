using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

    public Sprite openSprite;
    private SpriteRenderer sr;

    private bool isOpen;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}

    public void OpenChest() {
        Debug.Log("OPEN");
        isOpen = true;
        //sr.sprite = openSprite;
    }
}
