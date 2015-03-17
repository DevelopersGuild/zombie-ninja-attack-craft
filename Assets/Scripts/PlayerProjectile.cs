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
            //Calculate point of collision and knockback accordingly
            Vector3 contactPoint = other.transform.position;
            Vector3 center = transform.position;
            EnemyMoveController enemyMoveController = other.gameObject.GetComponent<EnemyMoveController>();
            if (enemyMoveController != null) {
                Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
                enemyMoveController.Knockback(pushDirection.normalized, 10000);
            }

            //Enemy takes damage
            Health enemyHealth = other.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(1);

            Destroy(gameObject);
        }
    }
}
