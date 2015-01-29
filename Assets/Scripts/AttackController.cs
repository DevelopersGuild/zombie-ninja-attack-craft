using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

    Animator animator;
    MoveController moveController;
    public AttackCollider attackCollider;

    public bool isAttacking;
    private bool alreadyAttacked;
    private Vector2 playerPosition;

	// Use this for initialization
	void Start () {
        isAttacking = false;
        animator = GetComponent<Animator>(); ;
        moveController = GetComponent<MoveController>();
	}
	
	// Update is called once per frame
	void Update () {
        playerPosition = moveController.transform.position;

        //Play attacking animations
        if (isAttacking) {
            animator.SetBool("IsAttacking", true);
        }
        else {
            animator.SetBool("IsAttacking", false);
        }

        //Move and rotate the collider relative to where the player is facing
        if (moveController.facing.x > 0) {
            attackCollider.transform.position = new Vector2(playerPosition.x + 0.5f, playerPosition.y);
            attackCollider.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (moveController.facing.x < 0) {
            attackCollider.transform.position = new Vector2(playerPosition.x - 0.5f, playerPosition.y);
            attackCollider.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (moveController.facing.y > 0) {
            attackCollider.transform.position = new Vector2(playerPosition.x, playerPosition.y + 0.5f);
            attackCollider.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveController.facing.y < 0) {
            attackCollider.transform.position = new Vector2(playerPosition.x, playerPosition.y - 0.5f);
            attackCollider.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
	}

    public void Attack() {
        //Set attack flags so it doesnt interfere with other animations
        isAttacking = true;
        moveController.isDashing = false;

        //Check for all the enemines in its colluder and deal damage to them
        if (attackCollider.enemiesInRange.Count > 0 && alreadyAttacked == false) {
            for (int i = 0; i < attackCollider.enemiesInRange.Count; i++) {
                Collider2D enemy = attackCollider.enemiesInRange[i] as Collider2D;
                EnemyHealth enemyHealth = enemy.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(1);
                if (enemyHealth.currentHealth <= 0) {
                    Debug.Log("Ded");
                    attackCollider.enemiesInRange.RemoveAt(i);
                }
            }
        }

        //Set flag so the player cant keep clicking and dealing damage 
        alreadyAttacked = true;
    }

    public void FinishedAttacking() {
        //Reset variables
        isAttacking = false;
        alreadyAttacked = false;
    }

    public bool CanAttack() {
        //Check if the player is allowed to attack
        if (moveController.isDashing) {
            return false;
        }
        else {
            return true;
        }
    }
}
