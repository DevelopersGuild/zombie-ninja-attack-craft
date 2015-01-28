using UnityEngine;
using System.Collections;
/* PlayerScript - Handle Input from a player */
public class Player : MonoBehaviour {
    private MoveController moveController;
    private Animator animator;
    private WeaponController weaponController;
    private AttackController attackController;

    void Awake() {
        moveController = GetComponent<MoveController>();
        animator = GetComponent<Animator>();
        weaponController = GetComponent<WeaponController>();
        attackController = GetComponent<AttackController>();
    }
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        /* Player Input */
        // Retrieve axis information from keyboard
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");



        //Move the player with the controller based off the players input
        if(Input.GetButtonDown("Jump")){
            moveController.Dash();
        }else{
            moveController.Move(inputX, inputY);
            //Debug.Log(inputX);
        }


        if (Input.GetButtonDown("Fire1") && attackController.CanAttack()) {
             attackController.Attack();
        }
    }
}