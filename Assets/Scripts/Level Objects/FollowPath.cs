using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour
{

     public enum FollowType
     {
          MoveTowards,
          Lerp
     }

     public FollowType type = FollowType.MoveTowards;
     public Pathway pathway;
     public float speed = 1f;
     public float maxDistanceToGoal = .1f;

     private IEnumerator<Transform> currentPoint;

     // Use this for initialization
     void Start()
     {
          if (pathway == null)
          {
               Debug.LogError("path is null", gameObject);
               return;
          }

          //Gets the path enumerator and MoveNext() is ran once so currentPoint.Current isnt null
          currentPoint = pathway.GetPathsEnumerator();
          currentPoint.MoveNext();

          if (currentPoint.Current == null) return;

          transform.position = currentPoint.Current.position;
     }

     void Update()
     {
          if (currentPoint == null || currentPoint.Current == null)
          {
               return;
          }

          //Check for the follow type of the path (which can be set in the inspector). Lerp or  MoveTowards depending on the path type
          if (type == FollowType.MoveTowards)
               transform.position = Vector3.MoveTowards(transform.position, currentPoint.Current.position, Time.deltaTime * speed);
          else if (type == FollowType.Lerp)
          {
               transform.position = Vector3.Lerp(transform.position, currentPoint.Current.position, Time.deltaTime * speed);
          }

          //Check if the platforms current distance is close enough to the goal. If so, Enumerate the path so the goal is now the next point.
          float distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;
          if (distanceSquared < maxDistanceToGoal * maxDistanceToGoal)
          {
               currentPoint.MoveNext();
          }
     }
}
