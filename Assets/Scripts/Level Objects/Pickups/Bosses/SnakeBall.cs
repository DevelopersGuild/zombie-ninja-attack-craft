﻿using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

public class SnakeBall : MonoBehaviour
{

     public Vector2 initialPos;
     private Vector3 direction;
     private EnemyMoveController moveController;
     public float pingpong, mDeg, y, x;
     public double t;
     public bool bite;
     public float magnitude;
     private bool momo;

     public void Start()
     {
          momo = true;
          magnitude = 100;
          moveController = GetComponent<EnemyMoveController>();
          t = 1.5;
          bite = false;
          initialPos = transform.position;
     }


     
     public void Update()
     {
          if (momo)
          {
               if (!bite)
               {
                    moveController.Move(0, 0);
                    mDeg = Mathf.Repeat(mDeg + (Time.deltaTime * 180), 360.0f);
                    float radians = mDeg * Mathf.Deg2Rad + 90;

                    Vector3 offset = new Vector3(Mathf.Sin(radians) / (72f / pingpong), 0, 0);
                    offset *= magnitude;
                    transform.position = transform.position + offset * Time.deltaTime;
               }
               else
               {
                    if (t <= 24)
                    {
                         transform.position += direction/2f;
                    }
                    else if (t <= 48)
                    {
                         transform.position -= direction/2f;
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

     public void dead()
     {
          Destroy(gameObject);
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


}