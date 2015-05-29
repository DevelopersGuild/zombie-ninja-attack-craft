using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

public class ZigZagProj : Projectile
{

     public float rotTime, rotAngle;

     public void start()
     {
          rotTime = 0.35f;
          rotAngle = 0;
     }

     // Update is called once per frame
     void Update()
     {
          if (AutoTileMap.Instance.GetAutotileCollisionAtPosition(transform.position) == AutoTileMap.eTileCollisionType.BLOCK)
          {
               Destroy(transform.gameObject);
          }

          if (rotTime > 0.35)
          {

               rotTime = 0;
               rotAngle *= -1;
               Quaternion q = Quaternion.AngleAxis(rotAngle, Vector3.forward);
               Shoot(rotAngle, q * currentVelocity);
          }
          rotTime += Time.deltaTime;
     }

     public void setRot(float rot)
     {
          rotAngle = rot;
     }

     public void setDir(Vector2 vec)
     {
          currentVelocity = vec * 0.2f;
          Shoot(rotAngle, currentVelocity);
     }



}
