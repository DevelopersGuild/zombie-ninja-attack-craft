using System;
using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
    //Angel enemy, robotic winged something that is fast and tanky.
    //Has one move:
    //1) Charge at player, then disappear and have multiple copies appear surrounding the player. 
    //One copy flies at the player and then the real angel re-appears.
    public class Angel : Enemy
    {
        public FakeAngel angelFakeObject;
        public RealAngel angelRealObject;

        private Health health;

        private bool isAgro, isFaking, stageThree, stageFour;

        private int rand;
        private float i;
        private Transform playerPos;
        private Vector2 distance;
        private Vector3 fakeVec;
        private double fake_CD, invis_CD, radius, running, idleTime;
        private Vector3 someVec;

        //private Animator animator;

        //Create the angel gameObject
        public void Start()
        {

            //animator = GetComponent<Animator>();
            moveController = GetComponent<EnemyMoveController>();
            health = GetComponent<Health>();
            player = FindObjectOfType<Player>();

            distance = new Vector2(0, 0);
            isAgro = false;
            isFaking = stageThree = stageFour = false;


            rnd = new System.Random(Guid.NewGuid().GetHashCode());
            t = 3 + rnd.Next(0, 3000) / 1000f;

            fake_CD = 0;
            invis_CD = 0;
            running = 0;

            radius = 1;
            i = 0;

        }

        public void Update()
        {
            rnd = new System.Random();
            currentX = transform.position.x;
            currentY = transform.position.y;

            //get distance and direction between angel and player
            if (player != null) {
                playerPos = player.transform;
                float xSp = player.transform.position.x - transform.position.x;
                float ySp = player.transform.position.y - transform.position.y;
                distance = playerPos.position - transform.position;
                direction = new Vector2(xSp, ySp);
            }

            //Check if player is in range of angel (aggression range)
            if (distance.magnitude <= AgroRange)
            {
                isAgro = true;

            }
            else
            {
                isAgro = false;
            }

            //check if angel is not already using it's #1 move and that it is sufficiently far away
            if (!isFaking && fake_CD < 0 && distance.magnitude > 1.4 && isAgro)
            {
                //turn move #1 on
                isFaking = true;
            }

            if(player != null) {
                   //if using move #1
                    if(isFaking) {
                        //move #1 goes on cooldown, wait for clones to disappear before reappearing
                        if (stageThree)
                        {
                            moveController.Move(0, 0);
                            if (running < 0)
                            {
                                if (invis_CD <= 0)
                                {
                                    isFaking = false;
                                    transform.position -= new Vector3(0, 0, 1);
                                    fake_CD = 10;
                                    stageThree = false;
                                }

                            }
                        }
                        //Charge at player while it is still far away
                        else if (distance.magnitude > 1.2)
                        {
                            if (transform.position.z == 0)
                            {
                                moveController.Move(direction.normalized, 2);
                            }
                        }
                        //When in range, stop, disappear, use move #1 (spawn clones)
                        else
                        {
                            moveController.Move(0, 0);

                            stageThree = true;
                            transform.position += new Vector3(0, 0, 1);
                            
                            rand = rnd.Next(1, 7);
                            InvokeRepeating("spawn", 0.0f, 0.1f);
                            running = 0.8;

                            invis_CD = 3;
                        }

                    }
                    else if (isAgro)
                    {
                         if (transform.position.z == 0)
                         {
                              moveController.Move(direction.normalized, 7);
                         }

                    }
                    else
                    {
                         if (idleTime > 0.4)
                         {
                              someVec = idle(t, rnd);
                              t = someVec.z;
                              idleTime = 0;
                         }
                         moveController.Move(someVec.x, someVec.y);
                    }

                    idleTime += Time.deltaTime;
                invis_CD -= Time.deltaTime;
                fake_CD -= Time.deltaTime;
                running -= Time.deltaTime;
                if (i == 8)
                {
                    i = 0;
                }
            }
        }
        public bool getAgro()
        {
            return isAgro;
        }

        public int currentHp()
        {
            return health.currentHealth;
        }

        public void onDeath()
        {

            //death animation
        }

        void spawn()
        {
            

            float tempX = (float)(playerPos.position.x + Math.Sin((Math.PI/4 * i)) * radius);
            float tempY = (float)(playerPos.position.y + Math.Cos((Math.PI/4 * i)) * radius);
            fakeVec = new Vector3(tempX, tempY, 0);

            Transform other = new GameObject().transform;
            other.rotation = Quaternion.Euler(0, 0, (float)(180-45 * i));
            Debug.Log("Hi I'm I" + i);
            if (i == rand)
            {
                RealAngel hwat = Instantiate(angelRealObject, fakeVec, other.rotation) as RealAngel;
            }
            else
            {
                FakeAngel hwat = Instantiate(angelFakeObject, fakeVec, other.rotation) as FakeAngel;
            }

            Debug.Log("Hey I'm ray" + rand);
            i++;
            if (i == 8)
            {
                
                CancelInvoke();
            } 
            
        }


    }
}


