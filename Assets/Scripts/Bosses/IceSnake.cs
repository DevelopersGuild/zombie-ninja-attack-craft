using System;
using UnityEngine;

public class IceSnake : SnakeBoss
{
     private Vector2 diffVec;
     private int attackChoice;

     public void Start()
     {

          //animator = GetComponent<Animator>();
          player = FindObjectOfType<Player>();
          moveController = GetComponent<EnemyMoveController>();
          health = GetComponent<Health>();
          health.cancelKnockback();

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
          mirrorSpawn = 0.5f;
          count = 1;
          attackChoice = 0;

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
                         if (cooldown_CD > 1.5)
                         {
                              cooldown_CD = 0;
                              moveController.Move(0, 0);
                              FireSnake fSnake = FindObjectOfType<FireSnake>();
                              if (fSnake != null && fSnake.fireBall_CD > 8 && iceBall_CD > 8)
                              {
                                   Debug.Log("HEY");
                                   attackChoice = 6;
                                   fSnake.setCombo();
                              }
                              else
                              {
                                   //Play animation after setting attackChoice, animation calls Attack();
                                   if (acid_CD > 9)
                                   {
                                        attackChoice = 1;
                                   }
                                   else if (spawn_CD > 8)
                                   {
                                        attackChoice = 2;
                                   }
                                   else if (laser_CD > 12)
                                   {
                                        attackChoice = 3;
                                   }
                                   else if (bite_CD > 10)
                                   {
                                        attackChoice = 4;
                                   }
                                   else if (fireTrail_CD > 9)
                                   {
                                        attackChoice = 5;
                                   }
                                   else if (iceBall_CD > 8 && fSnake == null)
                                   {
                                        attackChoice = 6;
                                   }
                                   else
                                   {
                                        cooldown_CD = 0.3f;
                                   }
                              }
                              Attack();
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

               }
          }
     }

     public void Attack()
     {
          if (attackChoice == 1)
          {
               acidAttack();
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
               biteAttack();
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
          laser.setLaserTwo(270, 170);
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
