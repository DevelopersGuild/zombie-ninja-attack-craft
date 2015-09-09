using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyOther : MonoBehaviour
{
     public float TimeToWait;
     public List<GameObject> hitlist = new List<GameObject>();

     // Use this for initialization
     void Start()
     {
          
     }

     // Update is called once per frame
     void Update()
     {
          if (TimeToWait < 0)
          {
               foreach (GameObject target in hitlist)
               {
                    Destroy(target);
               }
               Destroy(gameObject);
          }

          TimeToWait -= Time.deltaTime;
     }


}