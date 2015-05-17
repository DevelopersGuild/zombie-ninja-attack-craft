using System;
using UnityEngine;

namespace AssemblyCSharp {
    public class DummyEnemy : MonoBehaviour {
        private EnemyMoveController moveController;
        private Health health;

        System.Random rnd;

        private float currentX, currentY;
        private Vector2 distance, direction;
        private double t;

        //private Animator animator;

        public void Start() {
            //animator = GetComponent<Animator>();
            moveController = GetComponent<EnemyMoveController>();
            transform.gameObject.tag = "Attackable";
            health = GetComponent<Health>();

            t = 3;
        }

        public void Update() {
            rnd = new System.Random();

            if (t < 1) {
                if (GetComponent<Rigidbody2D>().velocity.magnitude != 0) {
                    //speed = new Vector2 (0, 0);
                    moveController.Move(0, 0);
                    t = 3;
                }
            }
            else if (t < 2 && t > 1.3) {
                int rand = rnd.Next(1, 5);
                if (rand == 1) {
                    //speed = new Vector2 (2, 0);
                    moveController.Move(1, 0, 5);
                    t = 1.3;
                }
                else if (rand == 2) {
                    //speed = new Vector2 (-2, 0);
                    moveController.Move(-1, 0, 5);
                    t = 1.3;
                }
                else if (rand == 3) {
                    //speed = new Vector2 (0, 2);
                    moveController.Move(0, 1, 5);
                    t = 1.3;
                }
                else if (rand == 4) {
                    //speed = new Vector2 (0, -2);
                    moveController.Move(0, -1, 5);
                    t = 1.3;
                }
            }
            t -= Time.deltaTime;

        }

        public int currentHp() {
            return health.currentHealth;
        }

        public void onDeath() {
            //death animation
        }
    }
}


