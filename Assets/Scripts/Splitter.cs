﻿using System;
using UnityEngine;

namespace AssemblyCSharp
{
     public class Splitter : Enemy
     {
          //private AnimationController animationController;
          private Health health;
          
          public Splitter splitObj;

          private bool isAgro;
          //private bool canJump;
          
          private Vector2 spawnDir;
          private double idleTime, spawnTime;
          private Vector3 someVec;
          
          //private Animator animator;
          
          [HideInInspector]
          public int generation;
          
          public void Start()
          {
               //animator = GetComponent<Animator>();
               
               moveController = GetComponent<EnemyMoveController>();
               //animationController = GetComponent<AnimationController>();
               health = GetComponent<Health>();
               player = FindObjectOfType<Player>();
               
               //rigidbody2D.mass = 10;
               isAgro = false;
               
               rnd = new System.Random(Guid.NewGuid().GetHashCode());
               t = 3 + rnd.Next(0, 3000) / 1000f;
               
               //Shrink with every generation
               float xFacing = transform.localScale.x < 0.0f ? -1.0f : 1.0f;
               float scale = 1.0f - 0.1f * generation;
               transform.localScale = new Vector3(scale * xFacing, scale, 1.0f);
          }
          
          public void Update()
          {
               if (spawnTime > 0)
               {
                    isInvincible = true;
                    spawnTime -= Time.deltaTime;
                    if (spawnTime <= 0)
                    {
                         isInvincible = false;
                    }
               }
               else
               {
                    // Debug.Log("LR: " + LRAttack.GetComponent<SpriteRenderer>().bounds.size.x);
                    // Debug.Log("UD: " + UDAttack.GetComponent<SpriteRenderer>().bounds.size.x);
                    checkInvincibility();
                    if (checkStun())
                    {
                         stunTimer -= Time.deltaTime;
                         moveController.Move(0, 0);
                    }
                    else
                    {
                         //basic aggression range formula
                         if (player)
                         {
                              Vector2 distance = player.transform.position - transform.position;
                              float xSp = distance.normalized.x;
                              float ySp = distance.normalized.y;
                              direction = new Vector2(xSp, ySp);
                              if (distance.magnitude <= AgroRange)
                              {
                                   isAgro = true;
                              }
                              if (distance.magnitude > AgroRange)
                              {
                                   isAgro = false;
                              }
                         }
                         else
                         {
                              isAgro = false;
                         }
                         
                         if (isAgro)
                         {
                              findPos();
                              //Ideally would have an animation then leaps to move
                              moveController.Move(direction / 8f);
                              
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
          }
          
          
          public bool getAgro()
          {
               return isAgro;
          }
          
          public int currentHp()
          {
               return health.currentHealth;
          }
          
          public override void onDeath()
          {
               if (generation < 2)
               {
                    float dx = player.transform.position.x - transform.position.x;
                    float dy = player.transform.position.y - transform.position.y;
                    Vector2 perpendicular = Math.Abs(dx) > Math.Abs(dy) ?
                         Vector2.up :
                         Vector2.left;
                    float distance = 0.2f;
                    Vector2 spawnOffset2 = perpendicular * distance;
                    Vector3 spawnOffset = new Vector3(spawnOffset2.x, spawnOffset2.y);

                    generation += 1;
                    Splitter split1 = Instantiate(splitObj, transform.position + spawnOffset * 1, transform.rotation) as Splitter;
                    Splitter split2 = Instantiate(splitObj, transform.position + spawnOffset * -1, transform.rotation) as Splitter;
                    split1.isInvincible = true;
                    split2.isInvincible = true;
                    split1.HelloWorld(0.5, generation);
                    split2.HelloWorld(0.5, generation);
               }
          }
          
          public void HelloWorld(double time, int gen)
          {
               spawnTime = time;
               generation = gen;
          }
          
     }
}
