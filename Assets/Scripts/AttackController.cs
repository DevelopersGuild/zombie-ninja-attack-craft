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
            moveController.canDash = false;
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

        //Debug.Log(alreadyAttacked);
	}

    public void Attack() {
        if (CanAttack()) {
            //Set attack flags so it doesnt interfere with other animations
            isAttacking = true;
            moveController.isDashing = false;
            moveController.canDash = false;

            //Check for all the enemines in its collider and deal damage to them
            if (attackCollider.enemiesInRange.Count > 0 && alreadyAttacked == false) {
                //Deal damage to all the enemies and knock them back
                for (int i = 0; i < attackCollider.enemiesInRange.Count; i++) {
                    GameObject enemy = attackCollider.enemiesInRange[i] as GameObject;
                    Health enemyHealth = enemy.gameObject.GetComponent<Health>();
                    enemyHealth.TakeDamage(1);

                    //Knockback according to where the player is
                    Vector3 contactPoint = enemy.collider2D.transform.position;
                    Vector3 center = attackCollider.transform.position;

                    MoveController enemyMoveController = enemy.gameObject.GetComponent<MoveController>();
                    if (enemyMoveController != null) {
                        Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
                        enemyMoveController.Knockback(pushDirection.normalized, 10000);
                    }

                }
                
                //Check if any of the enemies died. If they did, remove them from the list of enemies
                for (int i = 0; i < attackCollider.enemiesInRange.Count; i++) {
                    GameObject enemy = attackCollider.enemiesInRange[i] as GameObject;
                    Health enemyHealth = enemy.gameObject.GetComponent<Health>();
                    if (enemyHealth.currentHealth <= 0) {
                        attackCollider.enemiesInRange.RemoveAt(i);
                    }
                }
                alreadyAttacked = true;
            }

            //Set flag so the player cant keep clicking and dealing damage 
            alreadyAttacked = true;
        }

    }

    public void FinishedAttacking() {
        //Reset variables
        isAttacking = false;
        alreadyAttacked = false;
        moveController.canDash = true;
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
