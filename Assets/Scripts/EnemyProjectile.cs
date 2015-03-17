using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        //Check for enemy collision
        if (other.tag == "Player") {
            //Deal damage, knock back what it collided with, and destory itselfz
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.CalculateKnockback(other, transform.position);
            playerHealth.TakeDamage(1);

            Destroy(gameObject);
        }
    }

}
