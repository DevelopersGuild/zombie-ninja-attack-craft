using System;
using UnityEngine;

namespace AssemblyCSharp
{
     public class Porcupine : Enemy
     {

          private AnimationController animationController;

          public GameObject SparkParticle, SparkParticleInstance;
          public int SoundDistance;
          public float sparkTime;
          private float sparkTimer;
          private int sparksLeft;

          private Health health;

          private Vector2 distance;
          private Vector3 someVec;
          private double stop, idleTime;
          private bool canPlayerHearSpark;

          //private Animator animator;
          private CameraFollow camera;

          public void Start()
          {
               //animator = GetComponent<Animator>();
               camera = FindObjectOfType<CameraFollow>();
               player = FindObjectOfType<Player>();
               moveController = GetComponent<EnemyMoveController>();
               animationController = GetComponent<AnimationController>();
               transform.gameObject.tag = "Attackable";
               health = GetComponent<Health>();
               canPlayerHearSpark = false;

               rnd = new System.Random(Guid.NewGuid().GetHashCode());
               t = 3 + rnd.Next(0, 3000) / 1000f;

               distance = new Vector2(0, 0);
               stop = 1;

               sparksLeft = 0;
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
                         sparksLeft = 2;
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

          public float currentHp()
          {
               return health.currentHealth;
          }

          public void Spark()
          {
               if (sparksLeft-- > 0) {
                    CheckIfPlayerCanHearSpark();
                    moveController.Move(0, 0);
                    t = .5;
                    sparkTimer = sparkTime;
                    stop = 0;
                    GameObject spark = Instantiate(SparkParticle, transform.position, Quaternion.identity) as GameObject;
                    spark.transform.parent = transform;
               }
          }
          public void FinishedSpark()
          {
               animationController.isAttacking = false;
          }

          public void CheckIfPlayerCanHearSpark()
          {
               Vector3 cameraPosition2D = new Vector3(
                    camera.transform.position.x,
                    camera.transform.position.y
               );
               float distance = Vector3.Distance(transform.position, cameraPosition2D);
               if(distance < SoundDistance)
               {
                    GameManager.Notifications.PostNotification(this, "OnEnemySpark");
               }
          }
     }
}


