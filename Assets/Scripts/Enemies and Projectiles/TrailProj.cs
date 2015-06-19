using UnityEngine;
using System.Collections;

public class TrailProj : Projectile
{

     public GameObject trailObj, trail;
     public float t, trailTime;
     private Quaternion rot;

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
               float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg - 90;
               var q = Quaternion.AngleAxis(angle, Vector3.forward);
               trail = Instantiate(trailObj, transform.position, q) as GameObject;
               Destroy(trail, trailTime);
               trailTime -= 0.1f;
          }


     }


     //I must've have been dozing off when I made this method, keeping it here to remind me of my shame
     public void setDir(float ang, Vector2 dir, Quaternion q)
     {
          Shoot(ang, dir);
          transform.rotation = q;
     }




}
