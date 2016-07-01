using UnityEngine;
using System.Collections;

public class BossDoor : MonoBehaviour {

    public AudioClip unlockSound;
    public Sprite bossDoorSprite;
    public bool isBossDoor;
    private BoxCollider2D collider;
    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        if (isBossDoor)
            sr.sprite = bossDoorSprite;
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isBossDoor && GameManager.getBossKeys() >= 1)
            {
                OpenDoor();
                GameManager.subtractBossKeys(1);
            }
            else if (!isBossDoor && GameManager.getKeys() >= 1)
            {
                OpenDoor();
                GameManager.subtractKeys(1);
            }
        }
    }

    public void OpenDoor()
    {
        collider.enabled = false;
        sr.enabled = false;
    }
}
