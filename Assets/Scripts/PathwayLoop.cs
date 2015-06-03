using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathwayLoop : MonoBehaviour
{

     public Transform[] Points;

     public IEnumerator<Transform> GetPathsEnumerator()
     {
          //Check if the list of current Points is either null or has 0 elements
          if (Points == null || Points.Length < 1) yield break;

          int direction = 1;
          int index = 0;

          /*This gets ran whenever MoveNext() is called. This enumerates the list in a way that if the platform is at the end of its path, its direction gets reversed and
          the index of the list starts to loop backwards*/
          while (true)
          {
               yield return Points[index];

               //If the length of the Points is 1, then just make the platform stay there and do nothing
               if (Points.Length == 1) continue;

               if((index >= Points.Length - 1)) index = -1 ;
               Debug.Log(index);

               index++;
          }

     }

     //Similar to OnGUI() in the sense the when Unity sees this method it automatically gets ran. This allows us to see the line of the path while still in the scene
     public void OnDrawGizmos()
     {
          if (Points == null || Points.Length < 2) return;

          var points = Points.Where(t => t != null).ToList(); //Convert the array to an arraylist. Prevents crash from when a point get removed in the scene because of the null point
          if (points.Count < 2) return;

          for (int i = 1; i < points.Count; i++)
          {
               Gizmos.DrawLine(Points[i - 1].position, Points[i].position);
          }
     }
}
