using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

     private Animator animator;
     private EnemyMoveController moveController;
     public bool isAttacking;
     public bool isTeleporting;
     public bool gotAttacked;
     public bool facesMultiple;

	// Use this for initialization
	void Start () {
          moveController = GetComponent<EnemyMoveController>();
          animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

          if (animator != null)
          {
               if (facesMultiple)
               {
                    animator.SetFloat("movement_x", moveController.movementVector.x);
                    animator.SetFloat("movement_y", moveController.movementVector.y);

                    if (Mathf.Abs(moveController.movementVector.x) > Mathf.Abs(moveController.movementVector.y))
                    {
                         animator.SetFloat("facing_x", moveController.facing.x);
                         animator.SetFloat("facing_y", 0);
                    }
                    else
                    {
                         animator.SetFloat("facing_x", 0);
                         animator.SetFloat("facing_y", moveController.facing.y);
                    }
               }

               animator.SetBool("isMoving", moveController.isMoving);
               animator.SetBool("isAttacking", isAttacking);
               animator.SetBool("isTeleporting", isTeleporting);
          }
	}
}
