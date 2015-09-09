using System;
using UnityEngine;

public class ShieldBoss : Boss
{
     public Player player;
     public float AgroRange;

     public GameObject shieldObj;
     private GameObject shield;

     private EnemyMoveController moveController;
     private Health health;
     private SpriteRenderer sprRend;

     private bool isAgro;

     System.Random rnd;

     private Vector2 direction;
     private float maxVelocity, currentVelocity;

     //private Animator animator;

     private double multiplier;
     private float currentX, currentY, playerX, playerY, angle;

     private Vector3 left, right, up, down;
     private Quaternion west, east, north, south;



     public void Start()
     {

          //animator = GetComponent<Animator>();
          player = FindObjectOfType<Player>();
          moveController = GetComponent<EnemyMoveController>();
          health = GetComponent<Health>();
          isInvincible = false;
          isAgro = false;
          sprRend = GetComponent<SpriteRenderer>();
          maxVelocity = 1;
          currentVelocity = 0;

          left = new Vector3(-0.3f, 0, 0);
          right = new Vector3(0.3f, 0, 0);
          up = new Vector3(0, 0.3f, 0);
          down = new Vector3(0, -0.3f, 0);
          west = new Quaternion(0, 0, 90, 0);
          east = new Quaternion(0, 0, -90, 0);
          north = new Quaternion(0, 0, 0, 0);
          south = new Quaternion(0, 0, 180, 0);

          shield = Instantiate(shieldObj, transform.position, transform.rotation) as GameObject;

     }

     public void Update()
     {
          if (check())
          {

          }
          else if (player != null)
          {

               findPos();
               rnd = new System.Random();
               if (currentVelocity < maxVelocity)
               {
                    currentVelocity *= currentVelocity;
               }
               moveController.Move((direction * currentVelocity)/2);
               if (direction.x > direction.y)
               {
                    if (direction.x >= 0)
                    {
                         shield.transform.position = transform.position + right;
                         shield.transform.rotation = east;
                    }
                    else
                    {
                         shield.transform.position = transform.position + left;
                         shield.transform.rotation = west;
                    }
               }
               else
               {
                    if (direction.y >= 0)
                    {
                         shield.transform.position = transform.position + up;
                         shield.transform.rotation = north;
                    }
                    else
                    {
                         shield.transform.position = transform.position + down;
                         shield.transform.rotation = south;
                    }
               }
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

     private void findPos()
     {
          currentX = transform.position.x;
          currentY = transform.position.y;
          playerX = player.transform.position.x;
          playerY = player.transform.position.y;

          angle = Vector2.Angle(player.transform.position, transform.position);
          direction = new Vector2(playerX - currentX, playerY - currentY);
          direction = direction.normalized;
     }

     public bool check()
     {
          if (isInvincible)
          {
               timeSpentInvincible += Time.deltaTime;

               if (timeSpentInvincible < 1)
               {
                    blink = !blink;
                    GetComponent<Renderer>().enabled = blink;
                    moveController.canMove = false;
               }
               else if (timeSpentInvincible <= 2 + .2f * (18 - currentHp()))
               {
                    sprRend.color = new Color(200, 30, 50, 0.1f);
                    moveController.canMove = true;
               }
               else
               {
                    maxVelocity *= 1.1f;
                    currentVelocity = 1.00001f;
                    isInvincible = false;
                    timeSpentInvincible = 0;
                    GetComponent<Renderer>().enabled = true;
               }
          }
          return moveController.canMove;
          //previously isInvincible
     }

}

