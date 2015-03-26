using UnityEngine;
using System.Collections;

public class BombThrower : MonoBehaviour {

    public Transform playerPosition;
    public BombScript bomb;

    public BombScript bombObject;

    public float throwForce;

    private bool isAggroed;
    public float throwCooldown;
    private float currentTime;

	// Update is called once per frame
	void Update () {
        if (isAggroed) {
            //Throw a bomb with a cooldown towards the direction of the player if the player is within aggro range
            currentTime += Time.deltaTime;
            if (currentTime >= throwCooldown) {
                bombObject = Instantiate(bomb, transform.position, transform.rotation) as BombScript;
                Vector2 toPlayer = new Vector2(playerPosition.position.x - transform.position.x, playerPosition.position.y - transform.position.y);
                bombObject.rigidbody2D.velocity = (toPlayer * throwForce);
                currentTime = 0;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            isAggroed = true;
            currentTime = throwCooldown;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            isAggroed = false;
            currentTime = 0;
        }
    }


}
