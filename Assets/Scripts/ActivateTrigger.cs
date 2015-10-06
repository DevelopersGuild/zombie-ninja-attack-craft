using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ActivateTrigger : MonoBehaviour {

      public List<GameObject> objects = new List<GameObject>();

      void Start()
      {
            foreach (GameObject item in objects)
            {
                  item.SetActive(false);
                  Debug.Log(item.name);
            }
      }

      void OnTriggerEnter2D(Collider2D other)
      {
           if (other.tag == "Player")
           {
                foreach (GameObject item in objects)
                {
                     item.SetActive(true);
                }
           }
      }
}
