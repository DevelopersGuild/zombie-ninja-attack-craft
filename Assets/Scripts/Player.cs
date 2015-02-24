
using UnityEngine;
using System.Collections;
/* PlayerScript - Handle Input from a player */
public class Player : MonoBehaviour {
    //Components
    private MoveController moveController;
    private Animator animator;
    private AttackController attackController;
    private EnemyHealth playerHealth;

    //Double tap flags
    private float ButtonCooler;
    private int ButtonCount;
    private float lastTapTimeW;
    private float lastTapTimeS;
    private float lastTapTimeA;
    private float lastTapTimeD;
    private float tapSpeed;

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
        playerHealth = GetComponent<EnemyHealth>();
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
                moveController.Push(pushDirection.normalized, 10000);
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


        //if (moveController.gotAttacked) {
        //    attackedTimer -= Time.deltaTime;
        //}
        //if (attackedTimer < 0) {
        //    attackedTimer = .5f;
        //    moveController.FinishedGettingAttacked();
        //}
        //Debug.Log("attackedtimer" + attackedTimer + "gotattacked" + moveController.gotAttacked);

        //Move the player with the controller based off the players input
        if (Input.GetButtonDown("Jump")) {
            moveController.Dash();
        }
        else {
            moveController.Move(inputX, inputY);
            //Debug.Log(inputX + " <-x");
			//Debug.Log(inputY + " <-Y");
        }


        //Check for double tap      
        if (Input.GetKeyDown("w")) {
			moveController.newFacing = 1;
            if ((Time.time - lastTapTimeW) < tapSpeed) {
                moveController.Dash();
            }
            lastTapTimeW = Time.time;
        }
        if (Input.GetKeyDown("s")) {
			moveController.newFacing = 2;
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

        //Check for attack input
        if (Input.GetButtonDown("Fire1") && attackController.CanAttack()) {
            attackController.Attack();
        }



    }
}

