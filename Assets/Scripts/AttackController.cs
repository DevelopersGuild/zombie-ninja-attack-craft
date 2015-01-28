﻿using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

    Animator animator;
    MoveController moveController;
    public AttackCollider attackCollider;

    public bool isAttacking;
    private bool alreadyAttacked;

	// Use this for initialization
	void Start () {
        isAttacking = false;
        animator = GetComponent<Animator>(); ;
        moveController = GetComponent<MoveController>();
	}
	
	// Update is called once per frame
	void Update () {

        //Play attacking animations
        if (isAttacking) {
            animator.SetBool("IsAttacking", true);
        }
        else {
            animator.SetBool("IsAttacking", false);
        }

        //Move the collider relative to where the player is facing
        if (moveController.facing.x > 0) {
            attackCollider.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        }
        else if (moveController.facing.x < 0) {
            attackCollider.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
        }
        else if (moveController.facing.y > 0) {
            attackCollider.transform.position = new Vector2(transform.position.x, transform.position.y + 0.6f);
        }
        else if (moveController.facing.y < 0) {
            attackCollider.transform.position = new Vector2(transform.position.x, transform.position.y - 0.6f);
        }
	}

    public void Attack() {
        isAttacking = true;
        moveController.isDashing = false;

        //Check if there are any enemies in the collider TODO: maybe put all the enemies in the collider in an arraylist and deal damage to all of the,
        if (attackCollider.EnemyInRange()) {
            if (alreadyAttacked == false) Debug.Log("DO DAMAGE!");
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
