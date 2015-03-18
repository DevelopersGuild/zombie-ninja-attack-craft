using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    public Sprite onSprite;
    public Sprite offSprite;

    public float activeDuration;
    public bool isActive;
    public float timeOffset;
    private float timeSpentActive;

    public float butts;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        timeSpentActive -= timeOffset;
        isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
        timeSpentActive += Time.deltaTime;
        Debug.Log(timeSpentActive + "active" + isActive);
        if (timeSpentActive >= activeDuration) {
            Switch();
            timeSpentActive = 0;
        }

        if (isActive) {
            boxCollider.enabled = true;
            spriteRenderer.sprite = onSprite;
        }
        else {
            boxCollider.enabled = false;
            spriteRenderer.sprite = offSprite;
        }
	}

    void Switch() {
        if (isActive) {
            isActive = false;
        }else{
            isActive = true;
        }
    }
}
