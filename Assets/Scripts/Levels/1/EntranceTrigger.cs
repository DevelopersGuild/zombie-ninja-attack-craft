using UnityEngine;
using System.Collections;

public class EntranceTrigger : MonoBehaviour
{

     public Pathway spikePath;
     public GameObject spikeBlock;

     void OnTriggerEnter2D(Collider2D other)
     {
          Debug.Log("TIGGER");
          if (other.tag == "Player")
          {
               spikePath.gameObject.SetActive(true);
               spikeBlock.SetActive(true);
          }
     }
}
