using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

    // Components
    private Animator animator;
    public AttackController attackController;
    // Speed of object
    [Range(0, 10)]
    public float speed = 8;
    public float dashSpeed = 100;
    public float sdas;
    // Direction of object
    internal Vector2 direction;
    public Vector2 facing;
	public int newFacing;
    public enum facingDirection { up, right, down, left }
    internal Vector2 previousFacing;
    Vector2 previousMoving;
    // Actual movement
    internal Vector2 movementVector = new Vector2(0, 0);
    // Flags for state checking
    public bool isMoving;
    public bool isPressingMultiple;
    public bool isDashing;
    public bool canDash;
    public float dashIn;
    public bool canMove;
    public bool gotAttacked;


    void Awake() {
        attackController = GetComponent<AttackController>();
        animator = GetComponent<Animator>();

        isMoving = false;
        movementVector = new Vector2(0, 0);
        direction = new Vector2(0, 0);
        facing = new Vector2(0, -1);
        previousFacing = new Vector2(0, -1);

        canDash = true;
        canMove = true;
        isDashing = false;
        dashIn = 0;
    }

    // Update is called once per frame
    void Update() {
        //Subtract the cooldown for dashing and check when the player can dash again and whether or not its finished dashing
        dashIn -= Time.deltaTime;
        if (dashIn < 0.5) {
            isDashing = false;
            ToWalkPhysics();
        }
        if (dashIn < 0.2) {
            canDash = true;
            canMove = true;
        }

        //The player faces according to player input
        if (canMove) {
            if (newFacing == (int)facingDirection.up) {
                facing.y = 1;
                facing.x = 0;
            }
            else if (newFacing == (int)facingDirection.right) {
                facing.x = 1;
                facing.y = 0;
            }
            else if (newFacing == (int)facingDirection.down) {
                facing.y = -1;
                facing.x = 0;
            }
            else if (newFacing == (int)facingDirection.left) {
                facing.x = -1;
                facing.y = 0;
            }
        }


        //If the player is going diagonal, dont change the direction its facing
        if (facing.Equals(previousFacing) == false) {
            if (isPressingMultiple) {
                facing = previousFacing;
            }
        }


        //Check whether sprite is facing left or right. Flip the sprite based on its direction
        if (facing.x > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (facing.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Calculate movement amount
        movementVector = direction * speed;


		if (animator != null) {
			//Play walking animations
            animator.SetFloat("facing_x", facing.x);
            animator.SetFloat("facing_y", facing.y);
            animator.SetFloat("movement_x", movementVector.x);
            animator.SetFloat("movement_y", movementVector.y);
			if (isMoving == true) {
				animator.SetBool ("IsMoving", isMoving);
			} else {
				animator.SetBool ("IsMoving", false);
			}

			//Play dashing animations
			if (isDashing) {
				animator.SetBool ("IsDashing", true);
			} else {
				animator.SetBool ("IsDashing", false);
			}

        }

        //Check the players state. If its already doing something, prevent the player from being able to move
        if (isDashing || attackController.isAttacking) {
            canMove = false;
        }
        else {
            canMove = true;
        }

        previousFacing = facing;
        previousMoving = movementVector;
      
    }

    void FixedUpdate() {
        // Apply the movement to the rigidbody
        //rigidbody2D.AddForce(movementVector);

        if (canMove) {
            rigidbody2D.velocity = movementVector;
        }

        //Debug.Log("canDash:" + canDash + "   canAttack:" + attackController.CanAttack());
        //Debug.Log("speed:" + speed + "direction:" + direction + "movementVector" + movementVector);
    }

    /* Moves the object in a direction by a small amount (used for player input) */
    internal void Move(float input_X, float input_Y) {
        direction = new Vector2(input_X, input_Y);
    }

    /* Moves the object towards a destination by a small amount (used for enemy input)*/
    internal void Move(Vector2 _direction) {
        direction = _direction;
    }

    /*pushes a character in a direction by an amount*/
    internal void Push(Vector2 direction, float amount) {
        rigidbody2D.AddForce(direction * amount);
    }

    public void Knockback(Vector2 direction, float amount) {
        ToDashPhysics();
        rigidbody2D.AddForce(direction * amount);
        ToWalkPhysics();
    }

    public void Dash() {
        // Debug.Log("Facing:" + facing);
        // Only let the player dash if the cooldown is < 0. If he can, dash and reset the timer       
        if (canDash) {
            //Change these rigidbody parameters so the dashing feels better
            ToDashPhysics();
            rigidbody2D.velocity = facing * dashSpeed;
            Debug.Log(rigidbody2D.velocity);
            //Reset dash parameters
            dashIn = 1;
            isMoving = true;
            isDashing = true;
            canDash = false;
            canMove = false;
        }
    }

    public void GotAttacked() {
        gotAttacked = true;
    }

    public void FinishedGettingAttacked() {
        gotAttacked = false;
    }

    public void ToDashPhysics() {
        rigidbody2D.mass = 1;
        rigidbody2D.drag = 33;
    }

    public void ToWalkPhysics() {
        rigidbody2D.drag = 75;
        rigidbody2D.mass = 2;
    }






}
