using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour
{
     private float type, time;
     public Lightning light;
     public Lightning Thunder;

     // Use this for initialization
     void Start()
     {

     }

     public void set(int t)
     {
          type = t;
          time = t * 2.5f - 1;
          if (t == 2)
          {
               transform.localScale *= 3;
          }
     }

     // Update is called once per frame
     void Update()
     {
          //Debug.Log("Time is :" + time);
          if (time < 0)
          {

               if (type == 1)
               {

                    Lightning x = Instantiate(light, transform.position, transform.rotation) as Lightning;
                    Destroy(gameObject);
               }
               else if (type == 2)
               {
                    Lightning x = Instantiate(Thunder, transform.position, transform.rotation) as Lightning;
                    Destroy(gameObject);
               }
          }
          time -= Time.deltaTime;
     }

     void DestroySelf()
     {
          //animation
          Destroy(gameObject);
     }

}
