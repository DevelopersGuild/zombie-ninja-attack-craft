﻿using System;
using UnityEngine;

namespace AssemblyCSharp {
    public class Porcupine : MonoBehaviour {
        private Player player;
        public GameObject SparkParticle, SparkParticleInstance;
        public float sparkTime;

        private EnemyMoveController moveController;
        private Health health;

        System.Random rnd;

        private float currentX, currentY;
        private Transform playerPos;
        private Vector2 distance, direction;
        private double t, stop;

        //private Animator animator;

        public void Start() {
            //animator = GetComponent<Animator>();
            moveController = GetComponent<EnemyMoveController>();
            transform.gameObject.tag = "Attackable";
            health = GetComponent<Health>();
            player = FindObjectOfType<Player>();

            distance = new Vector2(0, 0);
            t = 3;
            stop = 1;
        }

        public void Update() {
            rnd = new System.Random();
            currentX = transform.position.x;
            currentY = transform.position.y;
            
            if (player != null) {
                if (sparkTime <= 0) {
                    moveController.Move(0, 0);
                    t = 2;
                    sparkTime = 6;
                    stop = 0;
                    Instantiate(SparkParticle, transform.position, Quaternion.identity);
                }


                if (stop < 1)
                {
                    moveController.Move(0, 0);
                }
                else if (t < 1)
                {
                    if (GetComponent<Rigidbody2D>().velocity.magnitude != 0) {
                        moveController.Move(0, 0);
                        t = 3;
                    }
                } 
                else if (t < 2 && t > 1.3) {
                    int rand = rnd.Next(1, 5);
                    if (rand == 1) {
                        moveController.Move(1, 0, 5);
                        t = 1.3;
                    }
                    else if (rand == 2) {
                        moveController.Move(-1, 0, 5);
                        t = 1.3;
                    }
                    else if (rand == 3) {
                        moveController.Move(0, 1, 5);
                        t = 1.3;
                    }
                    else if (rand == 4) {
                        moveController.Move(0, -1, 5);
                        t = 1.3;
                    }
                }
            }
            stop += Time.deltaTime;
            t -= Time.deltaTime;
            sparkTime -= Time.deltaTime;

        }

        public int currentHp() {
            return health.currentHealth;
        }

        public void onDeath() {
            //death animation
        }
    }
}


