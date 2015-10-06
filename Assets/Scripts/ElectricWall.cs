using UnityEngine;
using System.Collections;

     public class ElectricWall : MonoBehaviour
     {

          public float TimeToLive;
          private float knockback;
          public fakeElectricWall fakeWall;

          public void Start()
          {

          }

          public void Update()
          {
               TimeToLive -= Time.deltaTime;

               if (TimeToLive > 0)
               {
                    float remainder = TimeToLive % .1f;
                    //GetComponent<Renderer>().enabled = remainder > .05f;
               }
               else
               {
                    fakeElectricWall fWall = Instantiate(fakeWall, transform.position, transform.rotation) as fakeElectricWall;
                    Destroy(gameObject);
               }
          }

   


     }
