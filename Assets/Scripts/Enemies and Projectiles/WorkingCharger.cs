using UnityEngine;
using System;
using CreativeSpore;

public class WorkingCharger : Enemy
{
     //Positions
     //public Player player;
     //public float AgroRange;
     //public EnemyMoveController moveController;

     private Vector2 speed, distance, moveDir;

     //State checks
     private bool isAgro;
     private bool isTired;
     private bool isCharging;
     private bool check;

     //Components
     private Animator animator;
     private Rigidbody2D rb;
     //private MoveControllerNoAnimation moveControllerNoAnimation;

     private double timer, runTime, restTime, idleTime;
     private float tireTime;
     private Vector3 someVec;

     public void Start()
     {
          //moveControllerNoAnimation = GetComponent<MoveControllerNoAnimation> ();
          animator = GetComponent<Animator>();
          player = FindObjectOfType<Player>();
          rb = GetComponent<Rigidbody2D>();
          moveController = GetComponent<EnemyMoveController>();

          distance = new Vector2(0, 0);
          speed = new Vector2(0, 0);

          runTime = 0;
          restTime = 0;

          rnd = new System.Random(Guid.NewGuid().GetHashCode());
          t = 3 + rnd.Next(0, 3000) / 1000f;

          timer = 5;

          isAgro = false;
          check = true;
          isTired = false;
          isCharging = false;
          GetComponent<Rigidbody2D>().mass = 5;

     }

     void Update()
     {
          checkInvincibility();
          if (checkStun())
          {
               stunTimer -= Time.deltaTime;
               moveController.Move(0, 0);
          }
          else if (player != null)
          {
               rnd = new System.Random();
               distance = player.transform.position - transform.position;
               findPos();
               //Check distance between the player and charger. If its close enough, aggro

               if (isTired)
               {
                    canBlink = true;
                    restTime -= Time.deltaTime;
                    //play resting animation
                    if (restTime <= 0)
                    {
                         restTime = 1;
                         isTired = false;
                    }
               }
               else
               {
                    canBlink = false;
               }
               if (distance.magnitude <= AgroRange && !isTired && !isCharging)
               {

                    findPos();
                    if (distance.magnitude >= AgroRange - 1)
                    {
                         Walking();
                         moveController.Move(direction / 8);
                    }
                    else
                    {
                         moveController.Move(0, 0);
                         isCharging = true;
                    }
                    isAgro = true;


                    //animator.SetBool("isCharging", true);
               }
               // speed = new Vector2(0, 0);

               else if (distance.magnitude < (AgroRange - 1))
               {
                    if (isCharging)
                    {

                         Charge();
                         findPos();
                         checkPlayer(check);
                         check = false;
                         runTime += Time.deltaTime;
                         moveController.Move(moveDir * 2);
                         //Play Animation
                         //DoneCharging();
                         //speed = new Vector2(5 * xSpeed, 5 * ySpeed);
                         //speed = speed * 3;

                    }
               }

               //If the player isnt aggroed, it moves randomly
               else
               {
                    Walking();
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
               //temp -= Time.deltaTime;
          }
     }

     public void Walking()
     {
          animator.SetBool("isCharging", false);
          animator.SetBool("isTired", false);
         // animator.SetBool("ChargerIdle", true);
     }

     public void Charge()
     {
          animator.SetBool("isTired", false);
          //animator.SetBool("ChargerIdle", false);
          animator.Play("ChargerCharge");
          isInvincible = true;
     }

     public void Tired()
     {
          animator.SetBool("isCharging", false);
          animator.Play("ChargerTired");
     }

     public void DoneCharging()
     {
          moveController.canMove = false;
          isTired = true;
          isCharging = false;
          isInvincible = false;
          Tired();
          // animator.SetBool("isCharging", false);
          // animator.SetBool("isTired", true);
     }

     public void checkPlayer(bool bol)
     {
          if (bol)
          {
               moveDir = direction / 8;
          }
     }

     public void DoneResting()
     {
          isTired = false;
          animator.SetBool("isTired", false);
          check = true;
          moveController.canMove = true;
     }


     public void OnTriggerEnter2D(Collider2D other)
     {
          //Check for player collision
          if (other.gameObject.tag == "Player" || GetComponent<PhysicCharBehaviour>().IsColliding(transform.position))
          {
               if (isCharging)
               {
                    Tired();
               }
          }

     }

     public void onDeath()
     {
          //animation
     }
}


