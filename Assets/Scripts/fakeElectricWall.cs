using UnityEngine;
using System.Collections;

     public class fakeElectricWall : MonoBehaviour
     {

          public float TimeToLive;
          private float knockback;
          public ElectricWall realWall;

          public void Start()
          {

          }

          public void Update()
          {
               TimeToLive -= Time.deltaTime;

               if (TimeToLive > 0)
               {
                    //      float remainder = TimeToLive % .1f;
                    //GetComponent<Renderer>().enabled = remainder > .05f;
               }
               else
               {
                    ElectricWall fWall = Instantiate(realWall, transform.position, transform.rotation) as ElectricWall;
                    Destroy(gameObject);
               }
          }


     }