using UnityEngine;
using System.Collections;

public class ElectricField : MonoBehaviour
{
     private float time;

     // Use this for initialization
     void Start()
     {
          time = 4;
     }

     // Update is called once per frame
     void Update()
     {
          //animation
          //because no animation, using time
          if (time < 0)
          {
               Destroy(gameObject);
          }
          time -= Time.deltaTime;
     }

     void DestroySelf()
     {
          //animation
          Destroy(gameObject);
     }

}