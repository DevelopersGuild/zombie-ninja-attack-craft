using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        //Check for Player collision
        if (other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            Health playerHealth = other.GetComponent<Health>();

            //Deal damage, knockback, set the invinicility flag
            if (!player.isInvincible)
            {
                playerHealth.CalculateKnockback(other, transform.position);
                playerHealth.TakeDamage(1);
                player.isInvincible = true;
            }
            Destroy(gameObject);
        }
    }

}
