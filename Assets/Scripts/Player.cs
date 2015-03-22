
using UnityEngine;
using System.Collections;
/* PlayerScript - Handle Input from a player */
public class Player : MonoBehaviour {
    //Components
    private PlayerMoveController playerMoveController;
    private AttackController attackController;

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
	public bool isInvincible;
	public float timeSpentInvincible;
    public float attackedTimer;


    void Awake() {
		isInvincible = false;
		timeSpentInvincible = 1;
        playerMoveController = GetComponent<PlayerMoveController>();
        attackController = GetComponent<AttackController>();
        tapSpeed = .25f;
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
            playerMoveController.Dash();
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
            playerMoveController.isPressingMultiple = true;
        }
        else {
            playerMoveController.isPressingMultiple = false;
        }

        //Only move if the 2 buttons or less are being pressed
        if (keyCount < 3 && keyCount > 0) {

            //Handle double taps for dashing
            if (Input.GetKeyDown("w")) {
                if ((Time.time - lastTapTimeW) < tapSpeed) {
                    playerMoveController.Dash();
                }
                lastTapTimeW = Time.time;
            }
            if (Input.GetKeyDown("s")) {
                if ((Time.time - lastTapTimeS) < tapSpeed) {
                    playerMoveController.Dash();
                }
                lastTapTimeS = Time.time;
            }
            if (Input.GetKeyDown("a")) {
                if ((Time.time - lastTapTimeA) < tapSpeed) {
                    playerMoveController.Dash();
                }
                lastTapTimeA = Time.time;
            }
            if (Input.GetKeyDown("d")) {
                if ((Time.time - lastTapTimeD) < tapSpeed) {
                    playerMoveController.Dash();
                }
                lastTapTimeD = Time.time;
            }

            //Face the player depending on the button being pressed
            if (Input.GetKey("w")) {
                playerMoveController.newFacing = (int)MoveController.facingDirection.up;
            }
            if (Input.GetKey("s")) {
                playerMoveController.newFacing = (int)MoveController.facingDirection.down;

            }
            if (Input.GetKey("a")) {
                playerMoveController.newFacing = (int)MoveController.facingDirection.left;
            }
            if (Input.GetKey("d")) {
                playerMoveController.newFacing = (int)MoveController.facingDirection.right;
            }
            playerMoveController.isMoving = true;
            playerMoveController.Move(inputX, inputY);
        }
        else {
            playerMoveController.isMoving = false;
            playerMoveController.Move(0, 0);
        }
        keyCount = 0;
    }
}

