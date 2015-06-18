using System;
using UnityEngine;
using System.Collections;

public class HelicopterAI : Enemy
{
     public GameObject corner1, corner2, corner3, corner4;
     private Vector3[] locArr;

     

     public BombScript bombObj, mineObj;
     private BombScript bomb, mine;

     public WindupProjectile missileObj;
     private WindupProjectile missile;

     public Homer homingObj;
     private Homer homing;

     private Health health;

     private bool isAgro;

     private int loc;
     private Vector2 distance, distanceFromPoint;
     private double bomb_CD, mine_CD, cooldown, shot_CD, homing_CD;
     private Vector3 someVec;

     //private Animator animator;

     //Create the angel gameObject
     public void Start()
     {

          //animator = GetComponent<Animator>();
          moveController = GetComponent<EnemyMoveController>();
          health = GetComponent<Health>();
          player = FindObjectOfType<Player>();

          locArr = new Vector3[4];
          locArr[0] = corner1.transform.position;
          locArr[1] = corner2.transform.position;
          locArr[2] = corner3.transform.position;
          locArr[3] = corner4.transform.position;
          loc = 0;
          health.cancelKnockback();


          distance = new Vector2(0, 0);
          distanceFromPoint = new Vector2(10, 10);
          isAgro = false;

          shot_CD = 6;
          bomb_CD = 8;
          homing_CD = 8;
          mine_CD = 5;
          cooldown = 2;
     }

     public void Update()
     {

          rnd = new System.Random();
          checkInvincibility();
          if (checkStun())
          {
               stunTimer -= Time.deltaTime;
               moveController.Move(0, 0);
          }
          else if (player != null)
          {
               health.cancelKnockback();
               findPos();
               float xSp = player.transform.position.x - transform.position.x;
               float ySp = player.transform.position.y - transform.position.y;
               distance = player.transform.position - transform.position;

               if (distance.magnitude <= AgroRange)
               {
                    isAgro = true;

               }
               else
               {
                    isAgro = false;
               }
               distanceFromPoint = locArr[loc % 4] - transform.position;
               if (distanceFromPoint.magnitude < 0.3)
               {
                    loc++;
                    distanceFromPoint = locArr[loc % 4] - transform.position;
               }
               moveController.Move(distanceFromPoint.normalized/8);

               if (isAgro)
               {
                    if (cooldown >= 2)
                    {
                         cooldown = 0;
                         findPos();
                         //No prep animation, just has default spinning and creates projectiles
                         if (mine_CD > 5)
                         {
                              mine = Instantiate(mineObj, transform.position, transform.rotation) as BombScript;
                              mine_CD = 0;
                         }
                         if (homing_CD > 8)
                         {
                              homing = Instantiate(homingObj, transform.position, transform.rotation) as Homer;
                              homing_CD = 0;
                         }
                         else if (bomb_CD > 8)
                         {
                              //bomb = Instantiate(bombObj, transform.position, transform.rotation) as BombScript;
                             // bomb.GetComponent<Rigidbody>().velocity = direction * 6;
                              bomb_CD = 0;

                              bomb = Instantiate(bombObj, transform.position, transform.rotation) as BombScript;
                              Vector2 toPlayer = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
                              bomb.GetComponent<Rigidbody2D>().velocity = (toPlayer.normalized/3);
                         }
                         else if (shot_CD > 6)
                         {
                              float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                              var q = Quaternion.AngleAxis(angle, Vector3.forward);
                              missile = Instantiate(missileObj, transform.position, q) as WindupProjectile;
                              missile.Shoot(0,direction.normalized);
                              shot_CD = 0;
                         }
                         else
                         {
                              cooldown = 1;
                         }
                    }
               }
               bomb_CD += Time.deltaTime;
               shot_CD += Time.deltaTime;
               mine_CD += Time.deltaTime;
               homing_CD += Time.deltaTime;
               cooldown += Time.deltaTime;
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

     public void onDeath()
     {
          //Drop mine pick up
          //death animation
     }



}


