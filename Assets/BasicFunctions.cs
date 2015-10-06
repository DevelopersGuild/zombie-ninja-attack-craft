using System;
using UnityEngine;
using System.Collections;

public class BasicFunctions : MonoBehaviour
{

     // Use this for initialization
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {

     }

     public void Death()
     {
          Debug.Log("rip");
          Destroy(gameObject);
     }

}
