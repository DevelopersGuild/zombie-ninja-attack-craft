
using UnityEngine;
using System.Collections;
/* PlayerScript - Handle Input from a player */
public class Player : MonoBehaviour {
    //Components
    private MoveController moveController;
    private Animator animator;
    private AttackController attackController;
    private Health playerHealth;

    //Double tap flags
    private float ButtonCooler;
    private int ButtonCount;
    private float lastTapTimeW;
    private float lastTapTimeS;
    private float lastTapTimeA;
    private float lastTapTimeD;
    private float tapSpeed;

    private int keyCount;

    //Timers
	private bool isInvincible;
	private float timeSpentInvincible;
    private float attackedTimer;


    void Awake() {
		isInvincible = false;
		timeSpentInvincible = 1;
        moveController = GetComponent<MoveController>();
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
        playerHealth = GetComponent<Health>();
        tapSpeed = .15f;
    }

    public void OnCollisionStay2D(Collision2D other) {
        //Deal with enemy collision
        if (other.gameObject.CompareTag("Attackable")) {
            Collider2D enemyCollider = other.collider;
            //Take damage if the player isnt already currently invincible
            if (!isInvincible) {
                playerHealth.TakeDamage(1);
                isInvincible = true;
               
                //Knockback according to where the player was hit
                Vector3 contactPoint = other.contacts[0].point;
                Vector3 center = enemyCollider.bounds.center;

                Vector2 pushDirection = new Vector2(contactPoint.x - center.x, contactPoint.y - center.y);
                moveController.Knockback(pushDirection.normalized, 10000);
            }

        }
    }




    // Update is called once per frame
    void Update() {
        /* Player Input */
        // Retrieve axis information from keyboard
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        //Check for the invincibility timer. If the player is invincible, add to the timer. When the timers over, reset the flags
		if (isInvincible) {
			timeSpentInvincible += Time.deltaTime;

            if (timeSpentInvincible <= 3f) {
                float remainder = timeSpentInvincible % .3f;
                renderer.enabled = remainder > .15f;
            }else {
                isInvincible = false;
                timeSpentInvincible = 0;
                renderer.enabled = true;
            }
		}

        //The player acts according to input
        if (Input.GetButtonDown("Jump")) {
            moveController.Dash();
        }

        //Check for attack input
        if (Input.GetButtonDown("Fire1") && attackController.CanAttack()) {
            attackController.Attack();
        }

        if (Input.GetButtonDown("Fire2") && attackController.CanAttack()) {
            attackController.ShootProjectile();
        }


        //Check for how many keys are being pressed and act accordingly
        if (Input.GetKey("w")) {
            keyCount++;
        }
        if (Input.GetKey("s")) {
            keyCount++;
        }
        if (Input.GetKey("a")) {
            keyCount++;
        }
        if (Input.GetKey("d")) {
            keyCount++;
        }
        if (keyCount >= 2) {
            moveController.isPressingMultiple = true;
        }
        else {
            moveController.isPressingMultiple = false;
        }

        //Only move if the 2 buttons or less are being pressed
        if (keyCount < 3 && keyCount > 0) {

            //Handle double taps for dashing
            if (Input.GetKeyDown("w")) {
                if ((Time.time - lastTapTimeW) < tapSpeed) {
                    moveController.Dash();
                }
                lastTapTimeW = Time.time;
            }
            if (Input.GetKeyDown("s")) {
                if ((Time.time - lastTapTimeS) < tapSpeed) {
                    moveController.Dash();
                }
                lastTapTimeS = Time.time;
            }
            if (Input.GetKeyDown("a")) {
                if ((Time.time - lastTapTimeA) < tapSpeed) {
                    moveController.Dash();
                }
                lastTapTimeA = Time.time;
            }
            if (Input.GetKeyDown("d")) {
                if ((Time.time - lastTapTimeD) < tapSpeed) {
                    moveController.Dash();
                }
                lastTapTimeD = Time.time;
            }

            //Face the player depending on the button being pressed
            if (Input.GetKey("w")) {
                moveController.newFacing = (int)MoveController.facingDirection.up;
            }
            if (Input.GetKey("s")) {
                moveController.newFacing = (int)MoveController.facingDirection.down;

            }
            if (Input.GetKey("a")) {
                moveController.newFacing = (int)MoveController.facingDirection.left;
            }
            if (Input.GetKey("d")) {
                moveController.newFacing = (int)MoveController.facingDirection.right;
            }
            moveController.isMoving = true;
            moveController.Move(inputX, inputY);
        }
        else {
            moveController.isMoving = false;
            moveController.Move(0, 0);
        }
        keyCount = 0;
    }
}

