﻿using UnityEngine;
using System.Collections;

public class EnemyMoveController : MonoBehaviour {

    // Components
    private Animator animator;
    private Enemy enemy;
    // Speed of object
    [Range(0, 10)]
    public float speed = 8;
    // Direction of object
    internal Vector2 direction;
    public Vector2 facing;
    public enum facingDirection { up, right, down, left }
    // Actual movement
    internal Vector2 movementVector = new Vector2(0, 0);
    // Flags for state checking
    public bool isMoving;
    public bool canMove;
    public bool gotAttacked;
    //Knockback flags
    public bool isKnockedBack;
    public bool isStationary;
    private float recoveryTime;
    public float knockbackForce;
    private float knockBackTime;
    private float timeSpentKnockedBack;
    private Vector2 knockbackDirection;


    void Awake() {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        isMoving = false;
        movementVector = new Vector2(0, 0);
        direction = new Vector2(0, 0);
        facing = new Vector2(0, -1);

        knockbackForce = 4;
        isKnockedBack = false;
        timeSpentKnockedBack = 0;
        knockBackTime = 0.15f;
        recoveryTime = 0.5f;

        canMove = true;
    }

    // Update is called once per frame
    void Update() {

        if (recoveryTime > 0.5)
        {
            
        }
        else
        {
            /* Check if character is moving */
            if (GetComponent<Rigidbody2D>() != null)
            {
                if (GetComponent<Rigidbody2D>().velocity.normalized.x != 0 || GetComponent<Rigidbody2D>().velocity.normalized.y != 0)
                {
                    // Store the direction the player is facing in case they stop moving
                    facing = GetComponent<Rigidbody2D>().velocity.normalized;
                    isMoving = true;
                }
                if (movementVector.x == 0 && movementVector.y == 0)
                {
                    isMoving = false;
                }
            }

            //Check whether sprite is facing left or right. Flip the sprite based on its direction
            if (facing.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }


            // Calculate movement amount
            movementVector = direction * speed;

            //Change movement vector if they are being knocked back
            if (isKnockedBack && !isStationary)
            {
                timeSpentKnockedBack += Time.deltaTime;
                movementVector = knockbackDirection * knockbackForce;
                if (timeSpentKnockedBack >= knockBackTime)
                {
                    isKnockedBack = false;
                    timeSpentKnockedBack = 0;
                }
            }

            if (animator != null)
            {
                //Play walking animations
                animator.SetFloat("facing_x", facing.x);
                animator.SetFloat("facing_y", facing.y);
                animator.SetFloat("movement_x", movementVector.x);
                animator.SetFloat("movement_y", movementVector.y);
                if (isMoving == true)
                {
                    animator.SetBool("IsMoving", isMoving);
                }
                else
                {
                    animator.SetBool("IsMoving", false);
                }
            }
        }

    }

    void FixedUpdate() {
        if (canMove) {
            GetComponent<Rigidbody2D>().velocity = movementVector;
        }

        //Debug.Log("canDash:" + canDash + "   canAttack:" + attackController.CanAttack());
        //Debug.Log("speed:" + speed + "direction:" + direction + "movementVector" + movementVector);
    }

    /* Moves the object in a direction by a small amount (used for player input) */
    internal void Move(float input_X, float input_Y) {
        direction = new Vector2(input_X, input_Y);
    }

	internal void Move(float input_X, float input_Y, float input_Extra) {
		direction = new Vector2(input_X/input_Extra, input_Y/input_Extra);
	}

    /* Moves the object towards a destination by a small amount (used for enemy input)*/
    internal void Move(Vector2 _direction) {
        direction = _direction;
    }

	internal void Move(Vector2 _direction, float input_E) {
		direction = _direction / input_E;
	}
    //public void Knockback(Vector2 direction, float amount) {
    //    isKnockedBack = true;
    //    knockbackForce = amount;
    //    knockbackDirection = direction;
    //}

    public void Knockback(Vector2 direction) {
            isKnockedBack = true;
            knockbackDirection = direction;
    }


    public void GotAttacked() {
        gotAttacked = true;
    }

    public void FinishedGettingAttacked() {
        gotAttacked = false;
    }

    public void ToDashPhysics() {
        GetComponent<Rigidbody2D>().mass = 1;
        GetComponent<Rigidbody2D>().drag = 33;
    }

    public void ToWalkPhysics() {
        GetComponent<Rigidbody2D>().drag = 75;
        GetComponent<Rigidbody2D>().mass = 2;
    }

	public float getMove() {
		return movementVector.magnitude;
	}

	public void setSpd(float sp) {
		speed = sp;
	}

	public float getSpd() {
				return speed;
		}


}
