//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
     private Player dude;
     private Vector2 playerPosition;
     MoveController moveController;
     private Vector2 x;
     private Animator animator;
     private AttackController attackController;

     System.Random rnd;
     private double t;

     public void start()
     {
          x = new Vector2(0, 0);
          t = 10;
          moveController = GetComponent<MoveController>();
          GetComponent<Rigidbody2D>().mass = 5;
     }

     void Update()
     {
          rnd = new System.Random();
          //playerPosition = dude.moveController.transform.position;
          if (t <= 0.9)
          {
               if (GetComponent<Rigidbody2D>().velocity.magnitude != 0)
               {
                    x = new Vector2(0, 0);
                    t = 2;
                    //rand 1.5-3?
               }
               else
               {
                    //		if (playerPosition.magnitude - moveController.transform.position.magnitude > 10) {
                    int rand = rnd.Next(1, 5);
                    if (rand == 1)
                    {
                         x = new Vector2(2, 0);
                         t = 1;
                    }
                    else if (rand == 2)
                    {
                         x = new Vector2(-2, 0);
                         t = 1;
                    }
                    else if (rand == 3)
                    {
                         x = new Vector2(0, 2);
                         t = 1;
                    }
                    else
                    {
                         x = new Vector2(0, -2);
                         t = 1;
                    }

                    //Debug.Log (x.ToString ());
               }
          }

          else
          {
               t -= Time.deltaTime;
          }
          GetComponent<Rigidbody2D>().velocity = x;
          //this.transform.position = x;
     }

}

