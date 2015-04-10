﻿using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

    Animator animator;
    PlayerMoveController moveController;
    public BoxCollider2D attackCollider;

    public bool isAttacking;
    private bool alreadyAttacked;
    private Vector2 playerPosition;

    public Projectile PlayerArrow;
    public int ammo;

	// Use this for initialization
	void Start () {
        isAttacking = false;
        animator = GetComponent<Animator>(); ;
        moveController = GetComponent<PlayerMoveController>();
        attackCollider.GetComponent<Collider2D>().enabled = false;
        ammo = 5;
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
            attackCollider.transform.position = new Vector2(playerPosition.x + 0.25f, playerPosition.y);
            attackCollider.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (moveController.facing.x < 0) {
            attackCollider.transform.position = new Vector2(playerPosition.x - 0.25f, playerPosition.y);
            attackCollider.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (moveController.facing.y > 0) {
            attackCollider.transform.position = new Vector2(playerPosition.x, playerPosition.y + 0.25f);
            attackCollider.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveController.facing.y < 0) {
            attackCollider.transform.position = new Vector2(playerPosition.x, playerPosition.y - 0.25f);
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

            //Activate the attack collider so whatever was in the collider gets hurt
            attackCollider.GetComponent<Collider2D>().enabled = true;

            //Check for all the enemines in its collider and deal damage to them
            if (attackCollider.enemiesInRange.Count > 0 && alreadyAttacked == false) {
                //Deal damage to all the enemies and knock them back
                for (int i = 0; i < attackCollider.enemiesInRange.Count; i++) {
                    GameObject enemy = attackCollider.enemiesInRange[i] as GameObject;
                    Health enemyHealth = enemy.gameObject.GetComponent<Health>();
                    enemyHealth.TakeDamage(1);

                    //Knockback according to where the player is
                    Vector3 contactPoint = enemy.GetComponent<Collider2D>().transform.position;
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
        }
    }

    public void ShootProjectile(){
        if (ammo > 0) {
            ammo--;
            //Set Attack Flags
            isAttacking = true;
            moveController.isDashing = false;
            moveController.canDash = false;


            //Instantiate an arrow depending on which direction the player is facing
            if (moveController.facing.x > 0) {
                Projectile projectile = Instantiate(PlayerArrow, new Vector2(transform.position.x + 0.25f, transform.position.y), transform.rotation) as Projectile;
                projectile.Shoot(0, new Vector2(1, 0));
            }
            else if (moveController.facing.x < 0) {
                Projectile projectile = Instantiate(PlayerArrow, new Vector2(transform.position.x - 0.25f, transform.position.y), transform.rotation) as Projectile;
                projectile.Shoot(180, new Vector2(-1, 0));
            }
            else if (moveController.facing.y > 0) {
                Projectile projectile = Instantiate(PlayerArrow, new Vector2(transform.position.x, transform.position.y + 0.25f), transform.rotation) as Projectile;
                projectile.Shoot(90, new Vector2(0, 1));
            }
            else if (moveController.facing.y < 0) {
                Projectile projectile = Instantiate(PlayerArrow, new Vector2(transform.position.x, transform.position.y - 0.25f), transform.rotation) as Projectile;
                projectile.Shoot(-90, new Vector2(0, -1));
            }

            FinishedAttacking();
        }
    }

    public void FinishedAttacking() {
        //Reset variables
        isAttacking = false;
        alreadyAttacked = false;
        moveController.canDash = true;
        attackCollider.GetComponent<Collider2D>().enabled = false;
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
