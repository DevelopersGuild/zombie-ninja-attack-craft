using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour {

    public Sprite onSprite;
    public Sprite offSprite;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    public float activeDuration;
    public float timeOffset; //Should be about 0.5 - 1 seconds more than active duration
    private float unactiveDuration;
    private bool isActive;
    private float timeSpentActive;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        timeSpentActive -= timeOffset;
        isActive = false;
        unactiveDuration = activeDuration + 1;
	}
	
	// Update is called once per frame
	void Update () {
        timeSpentActive += Time.deltaTime;

        //If the spike is active, turn on the collider
        if (isActive) {
            boxCollider.enabled = true;
            spriteRenderer.sprite = onSprite;

            if (timeSpentActive >= activeDuration) {
                Switch();
                timeSpentActive = 0;
            }
        }

        //Otherwise the spike trap is off and harmless
        else {
            boxCollider.enabled = false;
            spriteRenderer.sprite = offSprite;

            if (timeSpentActive >= unactiveDuration) {
                Switch();
                timeSpentActive = 0;
            }
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
