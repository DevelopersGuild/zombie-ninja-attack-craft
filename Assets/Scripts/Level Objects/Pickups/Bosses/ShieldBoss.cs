using System;
using UnityEngine;

public class ShieldBoss : Boss
{
     public Player player;
     public float AgroRange;

     public shield shield;
     private shield shieldObj;

     public GameObject tow1, tow2, tow3, tow4, tow5, tow6, tow7, tow8, tow9;
     private GameObject[] towArr;

     private EnemyMoveController moveController;
     private Health health;
     private SpriteRenderer sprRend;

     private bool isAgro, setNext, move;

     System.Random rnd;

     private Vector2 direction;
     private float maxVelocity, currentVelocity;

     //private Animator animator;

     private double multiplier;
     private float currentX, currentY, playerX, playerY, angle, mult, w, e, n, s;
     private int currentTow;

     private Color rageCol, normCol, medCol;

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
          currentVelocity = 1f;

          left = new Vector3(-0.3f, 0, 0);
          right = new Vector3(0.3f, 0, 0);
          up = new Vector3(0, 0.3f, 0);
          down = new Vector3(0, -0.3f, 0);
          west = new Quaternion(0, 0, 90, 90);
          east = new Quaternion(0, 0, -90, 90);
          north = new Quaternion(0, 0, 0, 180);
          south = new Quaternion(0, 0, 180, 0);

          w = 90;
          e = -90;
          n = 0;
          s = 180;

          towArr = new GameObject[9];
          towArr[0] = tow1;
          towArr[1] = tow2;
          towArr[2] = tow3;
          towArr[3] = tow4;
          towArr[4] = tow5;
          towArr[5] = tow6;
          towArr[6] = tow7;
          towArr[7] = tow8;
          towArr[8] = tow9;

          foreach (GameObject tower in towArr)
          {
               tower.SetActive(false);
          }
          towArr[0].SetActive(true);
          currentTow = 1;
          setNext = false;
          //shield = Instantiate(shieldObj, transform.position, transform.rotation) as GameObject;

          rageCol = new Color(0.8f, 0.1f, 0.15f, 1f);
          normCol = new Color(1, 1, 1, 1f);
          medCol = new Color(0.4f, 0.35f, 0.2f, 0.8f);

          mult = 1.001f;
          move = true;

          health.canKnock = false;

     }

     public void Update()
     {
          health.canKnock = false;
          if (!check())
          {
               setNext = false;
          }
          else if (player != null)
          {
               
               setNext = true;
               findPos();
               rnd = new System.Random();
               if (currentVelocity < maxVelocity)
               {
                    currentVelocity *= mult;
                    //Debug.Log(currentVelocity);
               }
               if (move)
               {
                    moveController.Move((direction * currentVelocity) / 75f);
               }
               else
               {
                    moveController.Move(0, 0);
               }
               //transform.eulerAngles = new Vector3(0, 0, 2 * Mathf.Rad2Deg * angle);
               //Debug.Log("Dir " + direction);
               //Debug.Log("cV " + currentVelocity);
               if (Math.Abs(direction.x) > Math.Abs((direction.y * 2)))
               {
                    if (direction.x >= 0)
                    {
                         //shield.transform.position = transform.position + right;
                         //shield.transform.rotation = east;
                         shield.setRot(east);
                    }
                    else
                    {
                         // shield.transform.position = transform.position + left;
                         //shield.transform.rotation = west;
                         shield.setRot(west);
                    }
               }
               else
               {
                    if (direction.y >= 0)
                    {
                         //  shield.transform.position = transform.position + up;
                         //shield.transform.rotation = north;
                         shield.setRot(north);
                    }
                    else
                    {
                         // shield.transform.position = transform.position + down;
                         //shield.transform.rotation = south;
                         shield.setRot(south);
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

          angle = Vector2.Angle(transform.position, player.transform.position);
          direction = new Vector2(playerX - currentX, playerY - currentY);
          direction = direction.normalized;
     }

     public bool check()
     {

          if (currentHp() <= 0)
          {
               onDeath();
          }

          if (setNext)
          {
               towArr[(18 - (int)currentHp()) / 2].SetActive(true);
               setNext = false;

          }
          if (isInvincible)
          {
               timeSpentInvincible += Time.deltaTime;

               if (timeSpentInvincible < 1)
               {
                    sprRend.color = medCol;
                    move = false;
                    //play tired anim
               }
               else if (timeSpentInvincible <= 2 + .2f * (18 - currentHp()))
               {
                    sprRend.color = rageCol;
                    moveController.canMove = true;
                    move = true;
                    //actually do this at the end of animation, but no animations so math stuff
               }
               else
               {
                    maxVelocity *= 1.2f;
                    mult *= 1.01f;
                    currentVelocity = 1.001f;
                    isInvincible = false;
                    timeSpentInvincible = 0;
                    sprRend.color = normCol;
               }
          }
          return moveController.canMove;
          //previously isInvincible

     }

     public override void onDeath()
     {
          foreach (GameObject tower in towArr)
          {
               Destroy(tower);
          }
          //explode
          Destroy(gameObject);
     }

}

