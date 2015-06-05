using System;
using UnityEngine;

namespace AssemblyCSharp
{
     public class DummyEnemy : Enemy
     {
          private Health health;

          private Vector2 distance;
          private double idleTime;
          private Vector3 someVec;

          //private Animator animator;

          public void Start()
          {
               //animator = GetComponent<Animator>();
               moveController = GetComponent<EnemyMoveController>();
               transform.gameObject.tag = "Attackable";
               health = GetComponent<Health>();

               rnd = new System.Random(Guid.NewGuid().GetHashCode());
               t = 3 + rnd.Next(0, 3000) / 1000f;
          }

          public void Update()
          {
               checkInvincibility();
               rnd = new System.Random();
               if (checkStun())
               {
                    stunTimer -= Time.deltaTime;
                    moveController.Move(0, 0);
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


