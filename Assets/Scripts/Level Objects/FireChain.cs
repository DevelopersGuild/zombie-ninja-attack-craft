using UnityEngine;
using System.Collections;

public class FireChain : MonoBehaviour
{

     public float speed;
     public bool laserOne, laserTwo;
     private float rotation, fin;

     // Update is called once per frame
     void FixedUpdate()
     {
          rotation += speed;
          transform.eulerAngles = new Vector3(0, 0, rotation);

          if (rotation > 360)
          {
               rotation = 0;
          }
          if (laserOne)
          {
               if (rotation > fin)
               {
                    FindObjectOfType<FireSnake>().laserEnd();
                    Destroy(gameObject);
               }


          }
          else if (laserTwo)
          {
               if (rotation < fin)
               {
                    FindObjectOfType<IceSnake>().laserEnd();
                    Destroy(gameObject);
               }
          }

     }

     public void setLaserOne(float start, float finish)
     {
          rotation = start;
          fin = finish;
          laserOne = true;
     }

     public void setLaserTwo(float start, float finish)
     {
          rotation = start;
          fin = finish;
          speed *= -1;
          laserTwo = true;
     }
}
