using UnityEngine;
using System.Collections;

public class ArrowTrap : MonoBehaviour {

    public Projectile TrapArrow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("GOTCHABICH");
            Projectile projectile = Instantiate(TrapArrow, new Vector2(transform.position.x + 0.5f, transform.position.y +0.5f), transform.rotation) as Projectile;
            projectile.Shoot(0, new Vector2(0, 1));
            Debug.Log("whynothinginstantate");
        }
    }
}
