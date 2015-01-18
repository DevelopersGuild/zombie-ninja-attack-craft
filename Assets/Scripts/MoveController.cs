using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

    // Components
    private Animator animator;
    // Speed of object
    [Range(0, 500)]
    public float speed = 420;
    // Direction of object
    internal Vector2 direction;
    internal Vector2 facing;
    public Transform transform;
    // Actual movement
    internal Vector2 movementVector = new Vector2(0, 0);
    private bool isMoving;

    void Awake() {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        isMoving = false;
        movementVector = new Vector2(0, 0);
        direction = new Vector2(0, 0);
        facing = new Vector2(0, 0);
    }

	// Update is called once per frame
	void Update () {
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

        //Play animations
        if (isMoving == true) {
            animator.SetBool("IsMoving", isMoving);
            animator.SetFloat("movement_x", movementVector.x);
            animator.SetFloat("movement_y", movementVector.y);
        }
        else {
            animator.SetBool("IsMoving", false);
        }

    }
    void FixedUpdate() {
        // Apply the movement to the rigidbody
        rigidbody2D.AddForce(movementVector);

        //Debug.Log("speed:" + speed + "direction:" + direction + "movementVector" + movementVector);
    }
    /* Moves the object in a direction by a small amount (used for player input)
    float input_X: Input for X-axis movement (b/t -1 and 1)
    float input_Y: Input for Y-axis movement (b/t -1 and 1)
    */
    internal void Move(float input_X, float input_Y) {
        direction = new Vector2(input_X, input_Y);
    }
    /* Moves the object towards a destination by a small amount (used for enemy input)
    * Vector2 _direction: Direction to the destination
    */
    internal void Move(Vector2 _direction) {
        direction = _direction;
    }
    /* Push - pushes a character in a direction by an amount
    * Vector2 direction - The direction to push
    * float amount - The amount the character should be pushed
    */
    internal void Push(Vector2 direction, float amount) {
        rigidbody2D.AddForce(direction * amount);
    }
}
