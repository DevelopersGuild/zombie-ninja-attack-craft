using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 10;
    public int currentHealth;
    bool isDead;

    Animator anim;
    AudioSource enemyAudio;

	// Use this for initialization
	void Start () {
        currentHealth = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(int amount) {
        currentHealth -= amount;

        if (currentHealth <= 0) {
            Death();
        }
    }

    public void Death() {
        isDead = true;
        //anim.SetTrigger("Dead");
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
