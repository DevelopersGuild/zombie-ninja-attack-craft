using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        //Check for enemy collision
        if (other.tag == "Attackable") {
            //Deal damage, knock back what it collided with, and destory itselfz
            Health enemyHealth = other.gameObject.GetComponent<Health>();
            enemyHealth.CalculateKnockback(other, transform.position);
            enemyHealth.TakeDamage(1);

            Destroy(gameObject);
        }
    }
}
