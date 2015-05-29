using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
     public bool isInvincible, blink;
     public float timeSpentInvincible;

     public void start()
     {
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

}


