using System;
using UnityEngine;

namespace AssemblyCSharp
{

    public class IceSnake : SnakeBoss
    {


        private float currentX, currentY, playerX, playerY, angle;


        public void Start()
        {
            //animator = GetComponent<Animator>();

            bite_CD = 5;
            spawn_CD = 4;
            acid_CD = 6;
            fireBall_CD = 6;
            fireTrail_CD = 8;
            iceBall_CD = 6;
            iceTrail_CD = 8;
            laser_CD = 10;
            cooldown_CD = 0.8f;


        }

        public void Update()
        {
            if (player != null)
            {
                //find position after animation? will it use the position from before the animation starts? be ready to change
                findPos();

                rnd = new System.Random();

                distance = player.transform.position - transform.position;
                if (distance.magnitude <= AgroRange)
                {
                    isAgro = true;
                }
                if (distance.magnitude > AgroRange)
                {
                    isAgro = false;
                }

                if (isAgro)
                {
                    //targetPos *= 0.8f;
                    if (cooldown_CD > 0.8)
                    {
                        cooldown_CD = 0;
                        moveController.Move(0, 0);

                        if (acid_CD > 6)
                        {
                            acidAttack();
                            biteAttack();
                        }
                        else if (spawn_CD > 4)
                        {
                            spawnAttack();
                        }
                        else if (laser_CD > 10)
                        {
                            laserAttack();
                        }
                        else if (bite_CD > 5)
                        {
                            biteAttack();
                        }
                        else if (iceTrail_CD > 8)
                        {
                            trailAttack();
                        }
                        else if (iceBall_CD > 6)
                        {
                            ballAttack();
                        }

                        else
                        {
                            cooldown_CD = 0.6f;
                        }

                        //Ice Snake - Acid Ball -> Spawn Snakes -> Laser -> Bite -> ice trail -> iceball

                        //Loop with array for less code   
                        //attack
                    }

                }
                bite_CD += Time.deltaTime;
                laser_CD += Time.deltaTime;
                spawn_CD += Time.deltaTime;
                acid_CD += Time.deltaTime;
                fireBall_CD += Time.deltaTime;
                fireTrail_CD += Time.deltaTime;
                iceBall_CD += Time.deltaTime;
                iceTrail_CD += Time.deltaTime;
                cooldown_CD += Time.deltaTime;

                findPos();
            }
        }






    }
}