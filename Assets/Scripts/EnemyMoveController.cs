using UnityEngine;
using System.Collections;

public class EnemyMoveController : MonoBehaviour {

    // Components
    private Animator animator;
    private Transform transform;
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

    public bool isKnockedBack;
    public float knockedBackTime;
    public float timeKnockedBack;
    public Vector2 knockbackDirection;


    void Awake() {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();

        isMoving = false;
        movementVector = new Vector2(0, 0);
        direction = new Vector2(0, 0);
        facing = new Vector2(0, -1);

        canMove = true;
    }

    // Update is called once per frame
    void Update() {

        /* Check if character is moving */
        if (rigidbody2D.velocity.normalized.x != 0 || rigidbody2D.velocity.normalized.y != 0) {
            // Store the direction the player is facing in case they stop moving
            facing = rigidbody2D.velocity.normalized;
            isMoving = true;
        }
        if (movementVector.x == 0 && movementVector.y == 0) {
            isMoving = false;
        }

        //Check whether sprite is facing left or right. Flip the sprite based on its direction
        if (facing.x > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(-1, 1, 1);
        }


        // Calculate movement amount
        movementVector = direction * speed;

        if (isKnockedBack) {
            timeKnockedBack += Time.deltaTime;
            movementVector = knockbackDirection * 4;
            if (timeKnockedBack >= knockedBackTime) {
                isKnockedBack = false;
                timeKnockedBack = 0;
            }
        }

        if (animator != null) {
            //Play walking animations
            animator.SetFloat("facing_x", facing.x);
            animator.SetFloat("facing_y", facing.y);
            animator.SetFloat("movement_x", movementVector.x);
            animator.SetFloat("movement_y", movementVector.y);
            if (isMoving == true) {
                animator.SetBool("IsMoving", isMoving);
            }
            else {
                animator.SetBool("IsMoving", false);
            }
        }

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

    public void Knockback(Vector2 direction, float amount) {
        ToDashPhysics();
        isKnockedBack = true;
        rigidbody2D.AddForce(direction * amount, ForceMode2D.Force);
        ToWalkPhysics();
    }

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
        rigidbody2D.mass = 1;
        rigidbody2D.drag = 33;
    }

    public void ToWalkPhysics() {
        rigidbody2D.drag = 75;
        rigidbody2D.mass = 2;
    }






}
