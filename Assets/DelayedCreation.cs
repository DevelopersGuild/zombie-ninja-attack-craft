using System;
using UnityEngine;
using System.Collections;

public class DelayedCreation : MonoBehaviour
{
     public float TimeToWait;
     public GameObject obj;
     private GameObject ob;

     // Use this for initialization
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {
          if(TimeToWait < 0)
          {
               ob = Instantiate(obj, transform.position, transform.rotation) as GameObject;
               Destroy(gameObject);
          }

          TimeToWait -= Time.deltaTime;
     }

    
}
