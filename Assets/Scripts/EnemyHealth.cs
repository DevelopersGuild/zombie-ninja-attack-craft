using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 10;
    public int currentHealth;
    bool isDead;
    public int CoinValue;

    Animator anim;
    AudioSource enemyAudio;

	// Use this for initialization
	void Start () {
        currentHealth = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
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
            Debug.Log("DEATH");
            Death();
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
