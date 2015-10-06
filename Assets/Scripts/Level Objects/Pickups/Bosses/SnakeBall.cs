using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

public class SnakeBall : MonoBehaviour
{
     public GameObject explosion;
     public Vector2 initialPos;
     private Vector3 direction;
     private EnemyMoveController moveController;
     public float pingpong, mDeg, y, x, deadTime, time;
     public double t;
     public bool bite;
     public float magnitude;
     private bool momo;

     public void Start()
     {
          deadTime = -1f;
          momo = true;
          magnitude = 100;
          moveController = GetComponent<EnemyMoveController>();
          t = 1.5;
          bite = false;
          initialPos = transform.position;
     }



     public void Update()
     {
          if (deadTime <= -1)
          {
               if (momo)
               {
                    if (!bite)
                    {
                         moveController.Move(0, 0);
                         mDeg = Mathf.Repeat(mDeg + (time * 180), 360.0f);
                         float radians = mDeg * Mathf.Deg2Rad + 90;

                         Vector3 offset = new Vector3(Mathf.Sin(radians) / (72f / pingpong), 0, 0);
                         offset *= magnitude;
                         transform.position = transform.position + offset * time;
                    }
                    else
                    {
                         if (t <= 96)
                         {
                              transform.position += direction / 8f;
                         }
                         else if (t <= 192)
                         {
                              transform.position -= direction / 8f;
                         }
                         else
                         {
                              bite = false;
                              t = 0;
                         }
                         t++;
                    }
               }
          }
          else if(deadTime > 0)
          {
               deadTime -= Time.deltaTime;
          }
          else if(deadTime < 0)
          {
               death();
          }

     }

     public void dead(float t)
     {
          deadTime = t;
     }

     public void death()
     {
          GameObject expl = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
          Destroy(gameObject, 0);
     }

     public void setBite(bool bol, Vector3 vec)
     {
          bite = bol;
          y = vec.y;
          direction = vec;
          x = transform.position.x - vec.x;
          //mDeg = 180;
     }

     public void setPong(float p)
     {
          pingpong = p;
     }

     public void stopMove(bool mo)
     {
          momo = mo;
     }

     public void setT(float ti)
     {
          time = ti;
     }


}