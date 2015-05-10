/*
using System;
using UnityEngine;

namespace AssemblyCSharp
{
    public class Porcupine : MonoBehaviour
    {
        public Player player;
        public Explosion expl, explObject;

        private EnemyMoveController moveController;
        private Health health;

        System.Random rnd;

        private float currentX, currentY;
        private Transform playerPos;
        private Vector2 distance, direction;
        private double t, run_CD;

        //private Animator animator;


        public void Start()
        {
            //animator = GetComponent<Animator>();
            moveController = GetComponent<EnemyMoveController>();
            health = GetComponent<Health>();

            distance = new Vector2(0, 0);
            t = 3;
            run_CD = 0;
        }

        public void Update()
        {
            rnd = new System.Random();
            currentX = transform.position.x;
            currentY = transform.position.y;
            playerPos = player.transform;
            float xSp = player.transform.position.x - transform.position.x;
            float ySp = player.transform.position.y - transform.position.y;

            direction = new Vector2(xSp, ySp);

            if (player != null)
            {
                //basic aggression range formula
                distance = playerPos.position - transform.position;
                if(distance.magnitude < 1) {
                    //play animation
                    //in between slides change box collider size
                    expl = Instantiate(explObject, transform.position, transform.rotation) as Explosion;
                    run_CD = 3;
                }
                if(run_CD > 0) {
                    moveController.Move(direction.normalized, -5);
                }    
                else
                {
                    if (t < 1)
                    {
                        if (GetComponent<Rigidbody2D>().velocity.magnitude != 0)
                        {
                            moveController.Move(0, 0);
                            t = 3;
                        }
                    }
                    else if (t < 2 && t > 1.3)
                    {
                        int rand = rnd.Next(1, 5);
                        if (rand == 1)
                        {
                            moveController.Move(1, 0, 5);
                            t = 1.3;
                        }
                        else if (rand == 2)
                        {
                            moveController.Move(-1, 0, 5);
                            t = 1.3;
                        }
                        else if (rand == 3)
                        {
                            moveController.Move(0, 1, 5);
                            t = 1.3;
                        }
                        else if (rand == 4)
                        {
                            moveController.Move(0, -1, 5);
                            t = 1.3;
                        }
                    }
                }
                t -= Time.deltaTime;
                run_CD -= Time.deltaTime;
            }
            
        }

        public int currentHp()
        {
            return health.currentHealth;
        }

        public void onDeath()
        {
            //death animation
        }



    }
}


*/