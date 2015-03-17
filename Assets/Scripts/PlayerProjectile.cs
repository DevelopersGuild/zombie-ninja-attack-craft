using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {

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
