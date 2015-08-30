using UnityEngine;
using System.Collections;

public class EnemyAttack : Enemy {

     public BoxCollider2D upCol, downCol, leftCol, rightCol;
     public BoxCollider2D LR, UD;
     private BoxCollider2D attackCollider;
     public float yAttackRange;
     public float xAttackRange;

     public bool isAgro, canAttack, isAttacking;
     public bool playerInRange;
     private Vector2 attackDirection;
     public float attackingLength;
     private Vector2 distance, speed, facing, distanceFromPoint, point;
     private double idleTime, attackDelay;
     private Vector3 someVec;

     private Health health;
     private AnimationController animationController;

     public void Start()
     {
          //animator = GetComponent<Animator>();
          moveController = GetComponent<EnemyMoveController>();
          animationController = GetComponent<AnimationController>();
          health = GetComponent<Health>();
          player = FindObjectOfType<Player>();

          yAttackRange = 0.3f;
          xAttackRange = 0.3f;
          playerInRange = false;

          //rigidbody2D.mass = 10;
          distance = new Vector2(0, 0);
          speed = new Vector2(0, 0);
          isAgro = false;
          attackDelay = 3;

          rnd = new System.Random();
          t = 3 + rnd.Next(0, 3000) / 1000f;

          facing = new Vector2(0, 0);
          point = new Vector2(100, 100);
          distanceFromPoint = new Vector2(100, 100);
          canAttack = true;
          isAttacking = false;

     }
     public void Update()
     {
          checkInvincibility();

          // Freeze enemy if stunned
          if (checkStun())
          {
               stunTimer -= Time.deltaTime;
               moveController.Move(0, 0);
          }
          else
          {
               rnd = new System.Random();

               // Calculate direction to player
               if (player != null)
               {
                    distance = player.transform.position - transform.position;
               }

               float xSp = distance.normalized.x;
               float ySp = distance.normalized.y;
               direction = new Vector2(xSp, ySp);

               // Check for agro range
               if (distance.magnitude <= AgroRange)
               {
                    isAgro = true;
               }
               if (distance.magnitude > AgroRange)
               {
                    isAgro = false;
               }

               // Movement when aggroed to move towards
               if (isAgro && isAttacking == false)
               {
                    moveController.Move(direction / 6f);
               }
               // Dont move while attacking
               else if (isAttacking)
               {
                    moveController.Move(new Vector2(0, 0));
               }
               // Idle movement when not aggroed
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

               animationController.isAttacking = isAttacking;
               idleTime += Time.deltaTime;
               t -= Time.deltaTime;
          }
     }

     // Gets called when a player is in range of one of the attack colliders
     public void inRange(BoxCollider2D collision)
     {
          playerInRange = true;


          if (collision == upCol)
          {
               attackDirection = new Vector2(0, 1);
          }
          else if (collision == downCol)
          {
               attackDirection = new Vector2(0, -1);
          }
          else if (collision == leftCol)
          {
               attackDirection = new Vector2(-1, 0);
          }
          else
          {
               attackDirection = new Vector2(1, 0);
          }

          if(canAttack)
          {
               isAttacking = true;
          }
     }

     // Spawns the attack collider
     public void Attack()
     {

          if (attackDirection.y > 0)
          {
               attackCollider = Instantiate(UD, new Vector3(transform.position.x, transform.position.y + yAttackRange , transform.position.z), Quaternion.identity) as BoxCollider2D;
          }
          else if (attackDirection.y < 0)
          {
               attackCollider = Instantiate(UD, new Vector3(transform.position.x, transform.position.y - yAttackRange, transform.position.z), Quaternion.identity) as BoxCollider2D;
          }
          else if (attackDirection.x < 0)
          {
               if (transform.localScale.x == -1)
               {
                    attackCollider = Instantiate(LR, new Vector3(transform.position.x + xAttackRange, transform.position.y, transform.position.z), Quaternion.identity) as BoxCollider2D;
               }
               else
               {
                    attackCollider = Instantiate(LR, new Vector3(transform.position.x - xAttackRange, transform.position.y, transform.position.z), Quaternion.identity) as BoxCollider2D;
               }
          }
          else
          {
               if (transform.localScale.x == -1)
               {
                    attackCollider = Instantiate(LR, new Vector3(transform.position.x - xAttackRange, transform.position.y, transform.position.z), Quaternion.identity) as BoxCollider2D;
               }
               else
               {
                    attackCollider = Instantiate(LR, new Vector3(transform.position.x + xAttackRange, transform.position.y, transform.position.z), Quaternion.identity) as BoxCollider2D;
               }
          }
          attackCollider.transform.parent = transform;
          canAttack = false;
     }


     public void FinishedAttacking()
     {
          isAttacking = false;
          canAttack = true;
          Destroy(attackCollider.gameObject);
     }
}
