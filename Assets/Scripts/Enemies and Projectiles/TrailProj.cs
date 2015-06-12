using UnityEngine;
using System.Collections;

public class TrailProj : Projectile
{

     public GameObject trailObj, trail;
     public float t, trailTime;

     public void start()
     {
          t = 0;
          trailTime = 3;
     }

     // Update is called once per frame
     void Update()
     {
          t += Time.deltaTime;
          if (t > 0.075)
          {
               t = 0;
               trail = Instantiate(trailObj, transform.position, transform.rotation) as GameObject;
               Destroy(trail, trailTime);
               trailTime -= 0.1f;
               Debug.Log("HOOHOHOJSD");
          }

         
     }

     //I must've have been dozing off when I made this method, keeping it here to remind me of my shame
     public void setDir(float ang, Vector2 dir)
     {
          Shoot(ang, dir);
     }




}
