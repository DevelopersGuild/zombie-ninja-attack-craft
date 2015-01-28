﻿using UnityEngine;
using System.Collections;
/* PlayerScript - Handle Input from a player */
public class Player : MonoBehaviour {
    private MoveController moveController;
    private Animator animator;
    private WeaponController weaponController;
    private AttackController attackController;
    
    private float ButtonCooler;
    private int ButtonCount;
    private float lastTapTimeW;
    private float lastTapTimeS;
    private float lastTapTimeA;
    private float lastTapTimeD;
    private float tapSpeed;

    void Awake() {
        moveController = GetComponent<MoveController>();
        animator = GetComponent<Animator>();
        weaponController = GetComponent<WeaponController>();
        attackController = GetComponent<AttackController>();
        
        ButtonCooler = 0.5f;
        ButtonCount = 0;
        tapSpeed = .15f;
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


        //Check for double tap      
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