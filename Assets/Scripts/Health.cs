using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int startingHealth = 10;
    public int currentHealth;
    public int CoinValue;
    public bool isDead;

    private bool isQuitting;

    private Animator anim;
    private AudioSource enemyAudio;

	// Use this for initialization
	void Start () {
        isDead = false;
        currentHealth = startingHealth;
	}

	public void replenish(int amt) {
		currentHealth += amt;
		if (currentHealth > startingHealth)
			currentHealth = startingHealth;
	}

	public void setHealth(int amt) {
		currentHealth = amt;
	}

    public void TakeDamage(int amount) {
        currentHealth -= amount;

        if (currentHealth <= 0) {
            Death();
        }
    }

    //For triggers
    public void CalculateKnockback(Collider2D other, Vector2 currentPosition) {
        //Calculate point of collision and knockback accordingly
        Vector3 contactPoint = other.transform.position;
        Vector3 center = currentPosition;
        EnemyMoveController enemyMoveController = other.gameObject.GetComponent<EnemyMoveController>();
        PlayerMoveController playerMoveController = other.gameObject.GetComponent<PlayerMoveController>();

        if (enemyMoveController != null) {
            Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
            enemyMoveController.Knockback(pushDirection.normalized);
        }
        else {
            Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
            playerMoveController.Knockback(pushDirection.normalized);
        }

    }

    //For colliders
    public void CalculateKnockback(Collision2D other, Vector2 currentPosition) {
        //Calculate point of collision and knockback accordingly
        Vector3 contactPoint = other.transform.position;
        Vector3 center = currentPosition;
        EnemyMoveController enemyMoveController = other.gameObject.GetComponent<EnemyMoveController>();
        PlayerMoveController playerMoveController = other.gameObject.GetComponent<PlayerMoveController>();

        if (enemyMoveController != null) {
            Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
            enemyMoveController.Knockback(pushDirection.normalized);
        }
        else if(playerMoveController != null){
            Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
            playerMoveController.Knockback(pushDirection.normalized);
        }

    }

    public void Death() {
        isDead = true;
        Destroy(gameObject);
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }
    
    //Droop loot on death
    public void OnDestroy() {
        if (!isQuitting) {
            DropLoot dropLoot;
            if (dropLoot = GetComponent<DropLoot>()) {
                dropLoot.DropItem();
            }
        }
    }
}
