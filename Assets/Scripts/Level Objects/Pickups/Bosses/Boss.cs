using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
     public bool isInvincible, blink, shake;
     public float timeSpentInvincible, blinkTime, shakeTime;
     public Vector3 shakeDist;

     public void start()
     {
          shakeDist = new Vector3(1f, 0, 0);
          shakeTime = 5f;
          shake = false;
          isInvincible = false;

     }

     public void Update()
     {

     }

     public void checkInvincibility()
     {
          if (isInvincible)
          {
               timeSpentInvincible += Time.deltaTime;

               if (timeSpentInvincible <= 0.5f)
               {
                    blink = !blink;
                    GetComponent<Renderer>().enabled = blink;
               }
               else
               {
                    isInvincible = false;
                    timeSpentInvincible = 0;
                    GetComponent<Renderer>().enabled = true;
               }
          }
     }

     public void setBlink(float bl)
     {
          blinkTime = bl;
     }

     public virtual void onDeath()
     {
          Destroy(gameObject);
     }

     public void Shake()
     {
          shake = true;
     }

     public bool checkShake()
     {
          if (shake)
          {
               isInvincible = true;
               GetComponent<EnemyMoveController>().canMove = false;
               transform.position += shakeDist;
               //transform.position -= shakeDist * 2;
               shakeDist *= -1;
               shakeTime -= Time.deltaTime;
               if (shakeTime <= 0)
               {
                    onDeath();
               }
          }
          return shake;
     }

     public virtual void deathAnim()
     {

     }

}


