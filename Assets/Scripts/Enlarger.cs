using System;
using UnityEngine;

namespace AssemblyCSharp
{
     public class Enlarger : MonoBehaviour
     {
          private SpriteRenderer sprRend;
          private BoxCollider2D boxColl;
          public float enlargeInterval;
          public float enlargeAmount;
          private double time;

          public void Start()
          {
               sprRend = GetComponent<SpriteRenderer>();
               boxColl = GetComponent<BoxCollider2D>();
          }


          public void Update()
          {
               if (time > enlargeInterval)
               {
                    time = 0;
                    sprRend.transform.localScale *= enlargeAmount;
                    boxColl.transform.localScale *= enlargeAmount;
               }
               time += Time.deltaTime;
          }

   

     }
}



