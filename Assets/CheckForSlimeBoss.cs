using UnityEngine;
using System.Collections;

public class CheckForSlimeBoss : MonoBehaviour {

     private Splitter split;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

          if(!FindSplit())
          {
               Destroy(gameObject);
          }
          
	}

     bool FindSplit()
     {
          if (split = FindObjectOfType<Splitter>())
          {
               if (split.secretTag == 1)
               {
                    return true;
               }
               else
               {
                    FindSplit();
               }
          }
          return false;
     }
}
