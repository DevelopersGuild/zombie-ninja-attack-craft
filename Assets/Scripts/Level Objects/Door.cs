using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
     public bool isKey;

     SpriteRenderer sr;
     public BoxCollider2D collider;
     // Use this for initialization
     void Awake()
     {
          collider = GetComponent<BoxCollider2D>();
          sr = GetComponent<SpriteRenderer>();
     }

     // Open the door by disabling the collider and changin the sprite 
     public void OpenDoor()
     {
          collider.enabled = false;
          sr.enabled = false;
     }

     public void CloseDoor()
     {
          collider.enabled = true;
          sr.enabled = true;
     }
}
