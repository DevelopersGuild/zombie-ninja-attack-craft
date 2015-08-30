using UnityEngine;
using System.Collections;
using CreativeSpore;

public class EnemyMoveController : MonoBehaviour
{

     // Components
     private Animator animator;
     private Enemy enemy;
     // Speed of object
     [Range(0, 10)]
     public float speed = 8;
     // Direction of object
     internal Vector2 direction;
     public Vector2 facing;
     private Vector2 previousFacing;
     public enum facingDirection { up, right, down, left }
     // Actual movement
     public Vector2 movementVector = new Vector2(0, 0);
     // Flags for state checking
     public bool isMoving;
     public bool isAttacking;
     public bool canMove;
     public bool gotAttacked;
     //Knockback flags
     public bool isKnockedBack;
     public bool isStationary;
     private float recoveryTime;
     public float knockbackForce;
     private float knockBackTime;
     private float timeSpentKnockedBack;
     private Vector2 knockbackDirection;
     //Pathfinding
     private static float jiggleMax = 0.2f;
     private static Vector3 UL = new Vector3(-jiggleMax,  jiggleMax);
     private static Vector3 UR = new Vector3( jiggleMax,  jiggleMax);
     private static Vector3 DR = new Vector3( jiggleMax, -jiggleMax);
     private static Vector3 DL = new Vector3(-jiggleMax, -jiggleMax);
     private static float jiggleSpeed = 0.25f;


     void Awake()
     {
          animator = GetComponent<Animator>();
          enemy = GetComponent<Enemy>();

          isMoving = false;
          movementVector = new Vector2(0, 0);
          direction = new Vector2(0, 0);
          facing = new Vector2(0, -1);

          knockbackForce = 4;
          isKnockedBack = false;
          timeSpentKnockedBack = 0;
          knockBackTime = 0.15f;
          recoveryTime = 0.5f;

          canMove = true;
     }

     // Update is called once per frame
     void Update()
     {

          if (recoveryTime > 0.5)
          {

          }
          else
          {


               //Check whether sprite is facing left or right. Flip the sprite based on its direction
               if (facing.x > 0 != transform.localScale.x > 0)
               {
                    transform.localScale = new Vector3(
                         transform.localScale.x * -1.0f,
                         transform.localScale.y,
                         transform.localScale.z
                    );
               }


               // Calculate movement amount
               movementVector = direction * speed;

               /* Check if character is moving */
               if (GetComponent<Rigidbody2D>() != null)
               {
                    if (movementVector.x != 0 || movementVector.y != 0)
                    {
                         if (Mathf.Abs(movementVector.x) > Mathf.Abs(movementVector.y))
                         {
                              facing = movementVector.x < 0 ? new Vector2(-1, 0) : new Vector2(1,0);
                         }
                         else
                         {
                              facing = movementVector.y < 0 ? new Vector2(0, -1) : new Vector2(0, 1);
                         }
                         isMoving = true;
                    }
                    else
                    {
                         facing = previousFacing;
                         isMoving = false;
                    }
               }
               //Change movement vector if they are being knocked back
               if (isKnockedBack && !isStationary)
               {
                    timeSpentKnockedBack += Time.deltaTime;
                    movementVector = knockbackDirection * knockbackForce;
                    if (timeSpentKnockedBack >= knockBackTime)
                    {
                         isKnockedBack = false;
                         timeSpentKnockedBack = 0;
                    }
               }
               previousFacing = facing;

               //if (animator != null)
               //{
               //     //Play walking animations
               //     animator.SetFloat("facing_x", facing.x);
               //     animator.SetFloat("facing_y", facing.y);
               //     animator.SetFloat("movement_x", movementVector.x);
               //     animator.SetFloat("movement_y", movementVector.y);

               //     animator.SetBool("isMoving", isMoving);
               //}
          }

     }

     void FixedUpdate()
     {
          if (canMove)
          {
               GetComponent<Rigidbody2D>().velocity = JiggleMovement();
          }

          //Debug.Log("canDash:" + canDash + "   canAttack:" + attackController.CanAttack());
          //Debug.Log("speed:" + speed + "direction:" + direction + "movementVector" + movementVector);
     }

     
     /* Move around obstacles in the tile map if they are small */
     private Vector2 JiggleMovement()
     {
          PhysicCharBehaviour physics = GetComponent<PhysicCharBehaviour>();

          if (physics)
          {
               switch (physics.CollFlags)
               {
               case PhysicCharBehaviour.eCollFlags.LEFT:
                    return movementVector.y > 0 ? chooseJiggle(Vector2.up, UL, DL) : chooseJiggle(Vector2.up * -1, DL, UL);
               case PhysicCharBehaviour.eCollFlags.RIGHT:
                    return movementVector.y > 0 ? chooseJiggle(Vector2.up, UR, DR) : chooseJiggle(Vector2.up * -1, DR, UR);
               case PhysicCharBehaviour.eCollFlags.UP:
                    return movementVector.x > 0 ? chooseJiggle(-1 * Vector2.right, UL, UR) : chooseJiggle(Vector2.right, UR, UL);
               case PhysicCharBehaviour.eCollFlags.DOWN:
                    return movementVector.x > 0 ? chooseJiggle(-1 * Vector2.right, DL, DR) : chooseJiggle(Vector2.right, DR, DL);
               }
          }
          return movementVector;
     }

     private Vector2 chooseJiggle(Vector2 directionA, Vector2 a, Vector2 b)
     {
          PhysicCharBehaviour physics = GetComponent<PhysicCharBehaviour>();

          if (!physics.IsColliding(transform.position + new Vector3(a.x, a.y)))
          {
               return directionA * speed * jiggleSpeed;
          }
          else if (!physics.IsColliding(transform.position + new Vector3(b.x, b.y)))
          {
               return directionA * -1 * speed * jiggleSpeed;
          }
          return movementVector;
     }
     
     /* Moves the object in a direction by a small amount (used for player input) */
     internal void Move(float input_X, float input_Y)
     {
          direction = new Vector2(input_X, input_Y);
     }

     internal void Move(float input_X, float input_Y, float input_Extra)
     {
          direction = new Vector2(input_X / input_Extra, input_Y / input_Extra);
     }

     /* Moves the object towards a destination by a small amount (used for enemy input)*/
     internal void Move(Vector2 _direction)
     {
          direction = _direction;
     }

     internal void Move(Vector2 _direction, float input_E)
     {
          direction = _direction / input_E;
     }
     //public void Knockback(Vector2 direction, float amount) {
     //    isKnockedBack = true;
     //    knockbackForce = amount;
     //    knockbackDirection = direction;
     //}

     public void Knockback(Vector2 direction)
     {
          isKnockedBack = true;
          knockbackDirection = direction;
     }


     public void GotAttacked()
     {
          gotAttacked = true;
     }

     public void FinishedGettingAttacked()
     {
          gotAttacked = false;
     }

     public void ToDashPhysics()
     {
          GetComponent<Rigidbody2D>().mass = 1;
          GetComponent<Rigidbody2D>().drag = 33;
     }

     public void ToWalkPhysics()
     {
          GetComponent<Rigidbody2D>().drag = 75;
          GetComponent<Rigidbody2D>().mass = 2;
     }

     public float getMove()
     {
          return movementVector.magnitude;
     }

     public void setSpd(float sp)
     {
          speed = sp;
     }

     public float getSpd()
     {
          return speed;
     }


}
