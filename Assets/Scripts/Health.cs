using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int startingHealth = 10;
    public int asdf;
    public int currentHealth;
    bool isDead;
    public int CoinValue;

    Animator anim;
    AudioSource enemyAudio;

	// Use this for initialization
	void Start () {
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
        Debug.Log(currentHealth);

        if (currentHealth <= 0) {
            Death();
        }
    }

    public void CalculateKnockback(Collider2D other, Vector2 currentPosition) {
        //Calculate point of collision and knockback accordingly
        Vector3 contactPoint = other.transform.position;
        Vector3 center = currentPosition;
        EnemyMoveController enemyMoveController = other.gameObject.GetComponent<EnemyMoveController>();
        MoveController playerMoveController = other.gameObject.GetComponent<MoveController>();

        if (enemyMoveController != null) {;
            Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
            enemyMoveController.Knockback(pushDirection.normalized);
        }
        else {
            Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
            //playerMoveController.Knockback(pushDirection.normalized);
        }

    }
    public void CalculateKnockback(Collision2D other, Vector2 currentPosition) {
        //Calculate point of collision and knockback accordingly
        Vector3 contactPoint = other.transform.position;
        Vector3 center = currentPosition;
        EnemyMoveController enemyMoveController = other.gameObject.GetComponent<EnemyMoveController>();
        MoveController playerMoveController = other.gameObject.GetComponent<MoveController>();

        if (enemyMoveController != null) {
            Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
            enemyMoveController.Knockback(pushDirection.normalized, 3000);
        }
        else if(playerMoveController != null){
            Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
            playerMoveController.Knockback(pushDirection.normalized, 3000);
        }

    }

    public void Death() {
        isDead = true;
        if (GetComponent<DropCoinsOnDeath>()) {
            GetComponent<DropCoinsOnDeath>().DropCoins(CoinValue);
        }
        gameObject.SetActive(false);
    }
}
