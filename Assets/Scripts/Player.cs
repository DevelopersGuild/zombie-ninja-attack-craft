using UnityEngine;
using System.Collections;
/* PlayerScript - Handle Input from a player */
public class Player : MonoBehaviour {
    private MoveController moveController;
    private Animator animator;
    private WeaponController weaponController;

    void Awake() {
        moveController = GetComponent<MoveController>();
        animator = GetComponent<Animator>();
        weaponController = GetComponent<WeaponController>();
    }
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        /* Player Input */
        // Retrieve axis information from keyboard
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // Retrieve attack input
        bool attack = Input.GetButtonDown("Fire1");

        //Move the player with the controller based off the players input
        moveController.Move(inputX, inputY);

        if (attack) {

        }
    }
}