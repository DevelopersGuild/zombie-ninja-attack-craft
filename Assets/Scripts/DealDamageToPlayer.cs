using UnityEngine;
using System.Collections;

public class DealDamageToPlayer : MonoBehaviour {
    public void OnCollisionStay2D(Collision2D other)
    {
        //Deal with enemy collision
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            Player player = playerObject.GetComponent<Player>();
            Health playerHealth = playerObject.GetComponent<Health>();
            MoveController moveController = playerObject.GetComponent<MoveController>();
            Collider2D playerCollider = other.collider;

            //Take damage if the player isnt already currently invincible
            if (!player.isInvincible)
            {
                playerHealth.TakeDamage(1);
                player.isInvincible = true;

                //Knockback according to where the player was hit
                Vector3 contactPoint = other.contacts[0].point;
                Vector3 center = playerCollider.bounds.center;

                Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
                moveController.Knockback(pushDirection.normalized, 10000);
            }
        }
    }
}
