
using UnityEngine;
using System.Collections;
/* PlayerScript - Handle Input from a player */
public class Player : MonoBehaviour {
    private MoveController moveController;
    private Animator animator;
    private AttackController attackController;
	private EnemyHealth health;
    private float ButtonCooler;
    private int ButtonCount;
    private float lastTapTimeW;
    private float lastTapTimeS;
    private float lastTapTimeA;
    private float lastTapTimeD;
    private float tapSpeed;
	private bool isInvincible;
	private float invincibleTimer;

    void Awake() {
		isInvincible = false;
		invincibleTimer = 1;
        moveController = GetComponent<MoveController>();
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
		health = GetComponent<EnemyHealth> ();
        ButtonCooler = 0.5f;
        ButtonCount = 0;
        tapSpeed = .15f;
    }
    void Start() {

    }

	public void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Attackable")) {
			//other.damage();
			if(!isInvincible) {
				health.TakeDamage (1);
				moveController.Push (moveController.facing * -1, 10000);
				isInvincible = true;
			}
			
			//run invincibility
		}
	}

	public Vector2 getPos() {
				return moveController.transform.position;
		}

    // Update is called once per frame
    void Update() {
        /* Player Input */
        // Retrieve axis information from keyboard
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

		if (isInvincible) {
			invincibleTimer -= Time.deltaTime;
		}

		if (invincibleTimer <= 0) {
			invincibleTimer = 1;
			isInvincible = false;
		}


        //Move the player with the controller based off the players input
        if (Input.GetButtonDown("Jump")) {
            moveController.Dash();
        }
        else {
            moveController.Move(inputX, inputY);
            //Debug.Log(inputX + " <-x");
			//Debug.Log(inputY + " <-Y");
        }

       // Debug.Log("inputx" + inputX);


        //Check for double tap      
        if (Input.GetKeyDown("w")) {
			moveController.newFacing = 1;
			Debug.Log (moveController.newFacing + "hiiiiiii");
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

