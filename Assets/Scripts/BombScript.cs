using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {


    public Explosion explosion;

    public float timeToExplode;
    private float currentTime;

	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;

        //The sprite blinks once the player has stepped in its range and explodes afterwards
        if (currentTime <= timeToExplode) {
            float remainder = currentTime % .1f;
            renderer.enabled = remainder > .05f;
        }
        else {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
	}
}
