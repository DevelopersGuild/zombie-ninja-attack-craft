using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{

     private Animator animator;
     private EnemyMoveController moveController;
     public bool canFaceMultiple;
     public bool canTeleport;
     public bool canAttack;
     public bool isTeleporting;
     public bool isAttacking;
     public bool gotAttacked;


     // Use this for initialization
     void Start()
     {
          moveController = GetComponent<EnemyMoveController>();
          animator = GetComponent<Animator>();
     }

     // Update is called once per frame
     void Update()
     {

          if (animator != null)
          {
               if (canFaceMultiple)
               {
                    animator.SetFloat("movement_x", moveController.movementVector.x);
                    animator.SetFloat("movement_y", moveController.movementVector.y);
                    animator.SetFloat("facing_x", moveController.facing.x);
                    animator.SetFloat("facing_y", moveController.facing.y);

                    //if (Mathf.Abs(moveController.movementVector.x) > Mathf.Abs(moveController.movementVector.y))
                    //{
                    //     animator.SetFloat("facing_x", moveController.facing.x);
                    //     animator.SetFloat("facing_y", 0);
                    //}
                    //else
                    //{
                    //     animator.SetFloat("facing_x", 0);
                    //     animator.SetFloat("facing_y", moveController.facing.y);
                    //}


               }

               if (canAttack)
               {
                    animator.SetBool("isAttacking", isAttacking);
               }

               if (canTeleport)
               {
                    animator.SetBool("isTeleporting", isTeleporting);
               }

               animator.SetBool("isMoving", moveController.isMoving);
          }
     }
}
