using System;
using UnityEngine;

namespace AssemblyCSharp
{
     public class BasicEnemy : Enemy
     {
          public BasicAttack attackCollider, LRAttack, UDAttack;
          private Health health;

          private bool isAgro;

          private Vector2 distance, speed, facing;
          private double temp, idleTime, attackDelay;
          private Vector3 someVec;

          //private Animator animator;


          public void Start()
          {
               //animator = GetComponent<Animator>();

               moveController = GetComponent<EnemyMoveController>();
               health = GetComponent<Health>();
               player = FindObjectOfType<Player>();

               //rigidbody2D.mass = 10;
               distance = new Vector2(0, 0);
               speed = new Vector2(0, 0);
               isAgro = false;
               attackDelay = 1;

               rnd = new System.Random(Guid.NewGuid().GetHashCode());
               t = 3 + rnd.Next(0, 3000) / 1000f;

               facing = new Vector2(0, 0);

          }

          public void Update()
          {
               float xSp = player.transform.position.x - transform.position.x;
               float ySp = player.transform.position.y - transform.position.y;
               checkInvincibility();
               if (checkStun())
               {
                    stunTimer -= Time.deltaTime;
                    moveController.Move(0, 0);
               }
               rnd = new System.Random();
               if (player != null)
               {
                    //basic aggression range formula
                    distance = player.transform.position - transform.position;
                    if (distance.magnitude <= AgroRange)
                    {
                         isAgro = true;
                         //animator.SetBool("isCharging", true);
                    }
                    if (distance.magnitude > AgroRange)
                    {
                         isAgro = false;
                    }

                    if (isAgro)
                    {
                         if (attackDelay <= 0)
                         {
                              attackDelay = 1;
                              float xRot, yRot;
                              xRot = yRot = 0;
                              Debug.Log(xSp + " vs " + ySp);
                              if (Math.Abs(xSp) >= Math.Abs(ySp))
                                   attackCollider = Instantiate(LRAttack, transform.position + new Vector3(Math.Sign(xSp), 0, 0), Quaternion.identity) as BasicAttack;
                              else
                                   attackCollider = Instantiate(UDAttack, transform.position + new Vector3(0, Math.Sign(ySp), 0), UDAttack.transform.rotation) as BasicAttack;



                              //attackCollider.transform.rotation = Quaternion.identity;

                         }
                         else if (attackDelay > 0 && attackDelay < 1)
                         {
                              attackDelay -= Time.deltaTime;
                         }

                         else if (distance.magnitude < 1)
                         {
                              moveController.Move(0, 0);
                              attackDelay = 0.5;
                         }
                         else
                         {
                              direction = new Vector2(xSp, ySp);
                              moveController.Move(direction / 12f);
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
                    t -= Time.deltaTime;
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


     }
}
