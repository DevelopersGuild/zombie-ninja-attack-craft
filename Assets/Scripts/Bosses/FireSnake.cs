using System;
using UnityEngine;

     public class FireSnake : SnakeBoss
     {
          private Vector2 diffVec;


          public void Start()
          {

               //animator = GetComponent<Animator>();
               player = FindObjectOfType<Player>();
               moveController = GetComponent<EnemyMoveController>();
               health = GetComponent<Health>();
               health.knockbackMult = 0;

               isInvincible = true;
               bite_CD = 6;
               spawn_CD = 6;
               acid_CD = 6;
               fireBall_CD = 8;
               fireTrail_CD = 9;
               iceBall_CD = 8;
               iceTrail_CD = 9;
               laser_CD = 10;
               cooldown_CD = 0.8f;
               biteTime = 0;
               mirrorSpawn = -0.5f;
               count = 1;

               isAgro = false;

               diffVec = new Vector2(1, 1);

          }

          public void Update()
          {
               if (player != null)
               {
                    if (isBiting)
                    {
                         moveController.Move(0, 0);
                        // biteTime -= Time.deltaTime;
                        
                         findPos();
                         if (biteTime <= 12)
                         {
                              transform.position += biteDir;
                              
                         }
                         else if (biteTime <= 24 + count)
                         {
                              transform.position -= biteDir;
                         }
                         else
                         {
                              if (count > 0)
                              {
                                   count--;
                              }
                              moveController.Move(0, 0);
                              isBiting = false;
                              biteTime = 0;
                         }
                         biteTime++;
                    }
                    else if (isLasering)
                    {
                         moveController.Move(0, 0);
                         
                    }
                    else
                    {
                         //find position after animation? will it use the position from before the animation starts? be ready to change
                         findPos();
                         updatePos();

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

                                   if (bite_CD > 6)
                                   {
                                        biteAttack();
                                   }
                                   else if (spawn_CD > 6)
                                   {
                                        spawnAttack();
                                   }
                                   else if (laser_CD > 10)
                                   {
                                        laserAttack();
                                   }
                                   else if (acid_CD > 6)
                                   {
                                        acidAttack();
                                   }
                                   else if (fireTrail_CD > 9)
                                   {
                                         trailAttack();
                                   }
                                   else if (fireBall_CD > 8)
                                   {
                                         ballAttack();
                                   }

                                   else
                                   {
                                        cooldown_CD = 0.6f;
                                   }

                                   //Fire Snake - Bite -> Spawn Snakes -> -> Laser -> Acid Ball -> fire trail -> fireball

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

                    }
               }
          }

          public void laserAttack()
          {
               //After prep
               //animation
               laser = Instantiate(laserObj, transform.position, transform.rotation) as FireChain;
               laser.setLaserOne(90, 180);
               //create laser
               //after 0.5s, rotate around point from ~190 degrees to ~255 degrees
               //Ice snake mirrors that, from ~350 to ~285
               //laser ends
               laser_CD = 0;
               isLasering = true;
               b1.stopMove(false);
               b2.stopMove(false);
               b3.stopMove(false);
               b4.stopMove(false);
          }








     }
