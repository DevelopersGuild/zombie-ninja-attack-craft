﻿using System;
using UnityEngine;

public class FireSnake : SnakeBoss
{
     private Vector2 diffVec;
     private int attackChoice;
     private bool combo;

     public void Start()
     {

          animator = GetComponent<Animator>();
          player = FindObjectOfType<Player>();
          moveController = GetComponent<EnemyMoveController>();
          health = GetComponent<Health>();

          combo = false;
          isInvincible = true;
          bite_CD = 10;
          spawn_CD = 8;
          acid_CD = 9;
          fireBall_CD = 9;
          fireTrail_CD = 9;
          iceBall_CD = 9;
          iceTrail_CD = 9;
          laser_CD = 12;
          cooldown_CD = 0.8f;
          biteTime = 0;
          mirrorSpawn = -0.5f;
          count = 1;
          attackChoice = 0;
          isBiting = false;
          attackDelay = 0;

          isAgro = false;

          diffVec = new Vector2(1, 1);

          shakeTime = 1f;
          shakeDist = new Vector3(0.02f, 0, 0);

     }

     public void Update()
     {
          health.cancelKnockback();
          if (player != null)
          {
               if (!checkShake())
               {
                    if (blinkTime == 0.5f)
                    {
                         animator.SetBool("isOpen", false);
                    }
                    if (blinkTime > 0)
                    {
                         close();
                         blink = !blink;
                         GetComponent<Renderer>().enabled = blink;
                    }

                    else if (GetComponent<Renderer>().enabled == false)
                    {
                         GetComponent<Renderer>().enabled = true;
                    }

                    if (isBiting)
                    {
                         moveController.Move(0, 0);
                         // biteTime -= Time.deltaTime;

                         findPos();

                         if (biteTime <= 96)
                         {
                              transform.position += biteDir / 8f;

                         }
                         else if (biteTime <= 192 + count)
                         {
                              transform.position -= biteDir / 8f;
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
                              if (cooldown_CD > 1.5)
                              {
                                   cooldown_CD = 0;
                                   moveController.Move(0, 0);
                                   IceSnake iSnake = FindObjectOfType<IceSnake>();
                                   // if (iSnake != null && iSnake.iceBall_CD > 8 && fireBall_CD > 8)
                                   if (combo)
                                   {
                                        combo = false;
                                        attackChoice = 6;
                                        open();
                                   }
                                   if (attackDelay <= 0)
                                   {

                                        //Play animation after setting attackChoice, animation calls Attack();
                                        if (bite_CD > 15)
                                        {
                                             attackChoice = 1;
                                             open();
                                        }
                                        else if (spawn_CD > 18)
                                        {
                                             attackChoice = 2;
                                             open();
                                        }
                                        else if (laser_CD > 20)
                                        {
                                             attackChoice = 3;
                                             open();
                                        }
                                        else if (acid_CD > 14)
                                        {
                                             attackChoice = 4;
                                             open();
                                        }
                                        else if (fireTrail_CD > 13)
                                        {
                                             attackChoice = 5;
                                             open();
                                        }
                                        else if (iSnake == null && fireBall_CD > 8)
                                        {
                                             attackChoice = 6;
                                             open();
                                        }

                                        else
                                        {
                                             cooldown_CD = 0.3f;
                                        }
                                   }


                                   //Attack();
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
               else
               {
                    b1.stopMove(false);
                    b2.stopMove(false);
                    b3.stopMove(false);
                    b4.stopMove(false);
                    close();
                    cooldown_CD = -10;
               }
               blinkTime -= Time.deltaTime;
               attackDelay -= Time.deltaTime;
          }
     }

     public void Attack()
     {
          isInvincible = true;
          if (attackChoice == 1)
          {
               biteAttack();
          }
          else if (attackChoice == 2)
          {
               spawnAttack();
          }
          else if (attackChoice == 3)
          {
               laserAttack();
          }
          else if (attackChoice == 4)
          {
               acidAttack();
          }
          else if (attackChoice == 5)
          {
               trailAttack();
          }
          else if (attackChoice == 6)
          {
               ballAttack();
          }
          attackChoice = 0;

     }

     public void laserAttack()
     {
          //After prep
          //animation
          laser = Instantiate(laserObj, transform.position, transform.rotation) as FireChain;
          laser.setLaserOne(90, 170);
          //create laser
          //after 0.5s, rotate around point from ~190 degrees to ~255 degrees
          //Ice snake mirrors that, from ~350 to ~285
          //laser ends
          laser_CD = 0;
          isLasering = true;

          //stop balls or keep them moving?

          /*b1.stopMove(false);
          b2.stopMove(false);
          b3.stopMove(false);
          b4.stopMove(false);*/
     }

     public void setCombo()
     {
          combo = true;
          cooldown_CD = 1.6f;
     }







}
