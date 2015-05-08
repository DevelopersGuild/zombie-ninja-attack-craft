using UnityEngine;
using System.Collections;

public class PlayerMoveController : MonoBehaviour {

    // Components
    private Animator animator;
    public AttackController attackController;
    public ParticleSystem dashParticle;
    private ParticleSystem dashParticleInstance;
    // Speed of object
    [Range(0, 10)]
    public float speed = 8;
    public float dashSpeed = 100;
    // Direction of object
    internal Vector2 direction;
    public Vector2 facing;
    public int newFacing;
    public enum facingDirection { up, right, down, left }
    internal Vector2 previousFacing;
    // Actual movement
    internal Vector2 movementVector = new Vector2(0, 0);
    // Flags for state checking
    public bool isMoving;
    public bool isPressingMultiple;
    public bool isDashing;
    public bool canDash;
    public float dashIn;
    private bool dashCooldown;
    public bool canMove;
    public bool gotAttacked;
    //Knockback flags
    public bool isKnockedBack;
    public float knockbackForce;
    private float knockBackTime;
    private float timeSpentKnockedBack;
    private Vector2 knockbackDirection;


    void Awake() {
        attackController = GetComponent<AttackController>();
        animator = GetComponent<Animator>();

        isMoving = false;
        movementVector = new Vector2(0, 0);
        facing = new Vector2(0, -1);
        previousFacing = new Vector2(0, -1);

        canDash = true;
        canMove = true;
        isDashing = false;
        dashIn = 0;

        knockbackForce = 8;
        isKnockedBack = false;
        timeSpentKnockedBack = 0;
        knockBackTime = 0.12f;
    }

    // Update is called once per frame
    void Update() {
        //Subtract the cooldown for dashing and check when the player can dash again and whether or not its finished dashing
        dashIn -= Time.deltaTime;

        //Dash Cooldown
        if (dashIn < 0.1) {
            dashCooldown = true;
            ToWalkPhysics();
        }
        if (dashParticleInstance != null) {
            if (newFacing == (int)facingDirection.up) {
                dashParticleInstance.transform.position = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
            }
            else if (newFacing == (int)facingDirection.right) {
                dashParticleInstance.transform.position =  new Vector3(transform.position.x - 0.15f, transform.position.y - 0.15f, transform.position.z);
            }
            else if (newFacing == (int)facingDirection.down) {
                dashParticleInstance.transform.position =  new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
            }
            else {
                dashParticleInstance.transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y - 0.15f, transform.position.z);
            }
        }


        //The player can move after the dash cool down
        if (dashIn < -0.3) {
            canDash = true;
            isDashing = false;
            dashCooldown = false;
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


        // If the player is going diagonal, dont change the direction its facing
        if (facing.Equals(previousFacing) == false) {
            if (isPressingMultiple) {
                facing = previousFacing;
            }
        }


        // Check whether sprite is facing left or right. Flip the sprite based on its direction
        if (facing.x > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (facing.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Calculate movement amount
        movementVector = direction * speed;

        // Movement vector if its dashing
        if (isDashing) {
            movementVector = facing * dashSpeed;
        }
        
        //Change the movement vector to the knockbackvector if they are being knocked back
        if (isKnockedBack) {
            timeSpentKnockedBack += Time.deltaTime;
            movementVector = knockbackDirection * knockbackForce;
            if (timeSpentKnockedBack >= knockBackTime) {
                isKnockedBack = false;
                timeSpentKnockedBack = 0;
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

            //Play dashing animations
            if (isDashing) {
                animator.SetBool("IsDashing", true);
            }
            else {
                animator.SetBool("IsDashing", false);
            }
        }

        //Check the players state. If its already doing something, prevent the player from being able to move
        if (attackController.isAttacking || dashCooldown) {
            canMove = false;
        }
        else {
            canMove = true;
        }
        previousFacing = facing;
    }

    void FixedUpdate() {
        // Apply the movement to the rigidbody
        //rigidbody2D.AddForce(movementVector);
      
        if (canMove) {
            GetComponent<Rigidbody2D>().velocity = movementVector;
        }

        //Debug.Log("canDash:" + canDash + "   canAttack:" + attackController.CanAttack());
        //Debug.Log("speed:" + speed + "direction:" + direction + "movementVector" + movementVector);
        //Debug.Log(canMove);

    }

    /* Moves the object in a direction by a small amount (used for player input) */
    internal void Move(float input_X, float input_Y) {
        direction = new Vector2(input_X, input_Y);
    }

    /* Moves the object towards a destination by a small amount (used for enemy input)*/
    internal void Move(Vector2 _direction) {
        direction = _direction;
    }

    public void Dash() {
        // Debug.Log("Facing:" + facing);
        // Only let the player dash if the cooldown is < 0. If he can, dash and reset the timer       
        if (canDash) {
            // Change these rigidbody parameters so the dashing feels better
            ToDashPhysics();
            GetComponent<Rigidbody2D>().velocity = facing * dashSpeed;

            // Instantiate particle effects
            if (newFacing == (int)facingDirection.up) {
                dashParticleInstance = Instantiate(dashParticle, new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z), Quaternion.Euler(90,-90,90)) as ParticleSystem;
            }
            else if (newFacing == (int)facingDirection.right) {
                dashParticleInstance = Instantiate(dashParticle, new Vector3(transform.position.x - 0.15f, transform.position.y - 0.15f, transform.position.z), Quaternion.Euler(0, -90, 90)) as ParticleSystem;
            }
            else if (newFacing == (int)facingDirection.down) {
                dashParticleInstance = Instantiate(dashParticle, new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z), Quaternion.Euler(-90, 0, 0)) as ParticleSystem;
            }
            else {
                dashParticleInstance = Instantiate(dashParticle, new Vector3(transform.position.x + 0.15f, transform.position.y - 0.15f, transform.position.z), Quaternion.Euler(0, 90, -90)) as ParticleSystem;
            }

            //Reset dash parameters
            dashIn = .25f;
            isMoving = true;
            isDashing = true;
            canDash = false;
            canMove = true;
        }
    }

    public void Knockback(Vector2 direction) {
        isKnockedBack = true;
        knockbackDirection = direction;
    }

    public void GotAttacked() {
        gotAttacked = true;
    }
    
    // Gets called in the end of the animation
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






}
