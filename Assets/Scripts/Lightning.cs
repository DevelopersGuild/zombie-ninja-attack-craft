using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{
     public ElectricField area;
     public bool Thunder;

     //temporary
     private float temp;

     // Use this for initialization
     void Start()
     {
          temp = 1.5f;
     }

     // Update is called once per frame
     void Update()
     {
          //animation
          //because no animation, using time
          if (temp < 0)
          {
               if (Thunder)
               {
                    ElectricField x = Instantiate(area, transform.position, transform.rotation) as ElectricField;
               }
               Destroy(gameObject);
          }
          temp -= Time.deltaTime;
     }

     void DestroySelf()
     {
          //animation
          Destroy(gameObject);
     }

}