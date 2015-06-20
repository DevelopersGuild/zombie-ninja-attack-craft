using System;
using UnityEngine;

namespace AssemblyCSharp
{
     public class Porcupine : Enemy
     {

          private AnimationController animationController;

          public GameObject SparkParticle, SparkParticleInstance;
          public float sparkTime;
          private float sparkTimer;

          private Health health;

          private Vector2 distance;
          private Vector3 someVec;
          private double stop, idleTime;

          //private Animator animator;

          public void Start()
          {
               //animator = GetComponent<Animator>();
               player = FindObjectOfType<Player>();
               moveController = GetComponent<EnemyMoveController>();
               animationController = GetComponent<AnimationController>();
               transform.gameObject.tag = "Attackable";
               health = GetComponent<Health>();

               rnd = new System.Random(Guid.NewGuid().GetHashCode());
               t = 3 + rnd.Next(0, 3000) / 1000f;

               distance = new Vector2(0, 0);
               stop = 1;
          }

          public void Update()
          {
               checkInvincibility();
               if (checkStun())
               {
                    stunTimer -= Time.deltaTime;
                    moveController.Move(0, 0);
               }
               rnd = new System.Random();
               currentX = transform.position.x;
               currentY = transform.position.y;

               if (player != null)
               {
                    if (sparkTimer <= 0)
                    {
                         animationController.isAttacking = true;
                    }


                    if (stop < 1)
                    {
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
               }

               idleTime += Time.deltaTime;  
               stop += Time.deltaTime;
               t -= Time.deltaTime;
               sparkTimer -= Time.deltaTime;

          }

          public int currentHp()
          {
               return health.currentHealth;
          }

          public void Spark()
          {
               moveController.Move(0, 0);
               t = 2;
               sparkTimer = sparkTime;
               stop = 0;
               Instantiate(SparkParticle, transform.position, Quaternion.identity);
          }
          public void FinishedSpark()
          {
               animationController.isAttacking = false;
          }
     }
}


