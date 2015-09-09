﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public class EnemyHugger : Enemy
{
     private Health health;
     private Rigidbody2D rigid;

     public bool isSnake;
     
     private bool isAgro;

     private Transform playerPos;
     private Vector2 distance, speed, facing;
     private double temp, fireBlock_CD, idleTime;
     private Vector3 someVec, point;

     //private Animator animator;


     public void Start()
     {
          //animator = GetComponent<Animator>();
          moveController = GetComponent<EnemyMoveController>();
          health = GetComponent<Health>();
          player = FindObjectOfType<Player>();
          rigid = GetComponent<Rigidbody2D>();

          distance = new Vector2(0, 0);
          speed = new Vector2(0, 0);
          isAgro = false;

          rnd = new System.Random(Guid.NewGuid().GetHashCode());
          t = 3 + rnd.Next(0, 3000) / 1000f;

          //temp is the number for exponential speed when running away
          temp = 1.0000001;
          facing = new Vector2(0, 0);

     }

     public void Update()
     {
          checkInvincibility();
          if (checkStun())
          {
               stunTimer -= Time.deltaTime;
               moveController.Move(0, 0);
          }// Check existence of player. Move the enemy if aggroed, otherwise move randomly
          else if (player != null)
          {
               
               rnd = new System.Random();
               findPos();
               //basic aggression range formula
               playerPos = player.transform;
               float xSp = player.transform.position.x - transform.position.x;
               float ySp = player.transform.position.y - transform.position.y;
               direction = new Vector2(xSp, ySp);

               distance = playerPos.position - transform.position;
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
                    moveController.Move(direction.normalized, 8);
                    point = direction;
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
                         point = new Vector3(someVec.x, someVec.y, 0);                   
               }

               idleTime += Time.deltaTime;
               t -= Time.deltaTime;
          }

          if (health.currentHp() == 0)
          {
               onDeath();
          }
          if (isSnake)
          {
               float angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg - 90;
               Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
               transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2);
 
          }
     }


     public bool getAgro()
     {
          return isAgro;
     }

     public float currentHp()
     {
          return health.currentHealth;
     }

     public void onDeath()
     {
          //play pre-explosion animation
         // Destroy(gameObject);
          //death animation
     }


}



