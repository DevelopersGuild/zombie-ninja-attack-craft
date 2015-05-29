using System;
using UnityEngine;

namespace AssemblyCSharp
{
     public class WindCannon : MonoBehaviour
     {
          public Player player;
          public float AgroRange;
          public Projectile wind, windObject;

          private Health health;

          private bool isAgro;

          private double shot_CD;
          public Vector2 direction, distance;

          //private Animator animator;


          public void Start()
          {
               //animator = GetComponent<Animator>();

               isAgro = false;
               shot_CD = 1;

          }

          public void Update()
          {
               if (player != null)
               {

                    distance = player.transform.position - transform.position;
                    if (distance.magnitude <= AgroRange)
                    {
                         isAgro = true;
                    }
                    if (distance.magnitude > AgroRange)
                    {
                         isAgro = false;
                         shot_CD = 0;
                    }

                    if (isAgro)
                    {
                         if (shot_CD < 0)
                         {
                              shot_CD = 1;
                              wind = Instantiate(windObject, transform.position, transform.rotation) as Projectile;
                         }
                         shot_CD -= Time.deltaTime;
                    }
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


     }
}


