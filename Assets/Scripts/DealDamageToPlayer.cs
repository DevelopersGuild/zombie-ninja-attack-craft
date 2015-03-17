using UnityEngine;
using System.Collections;

public class DealDamageToPlayer : MonoBehaviour {

    BoxCollider2D collider;
    public void OnCollisionStay2D(Collision2D other)
    {
        //Check for player collision
        if (other.gameObject.tag == "Player")
        {
            //Find components necessary to take damage and knockback
            GameObject playerObject = other.gameObject;
            Player player = playerObject.GetComponent<Player>();
            Health playerHealth = playerObject.GetComponent<Health>();
            MoveController moveController = playerObject.GetComponent<MoveController>();

            //Take damage if the player isnt already currently invincible
            if (!player.isInvincible)
            {
               //Deal damage, knockback, set the invinicility flag
                playerHealth.CalculateKnockback(other, transform.position);
                playerHealth.TakeDamage(1);
                player.isInvincible = true;
            }
        }
    }
}
