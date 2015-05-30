using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{

     public Sprite opened;
     public Sprite closed;

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
          if (opened != null)
          {
               sr.sprite = opened;
          }
          else
          {
               sr.enabled = false;
          }
     }

     public void CloseDoor()
     {
          collider.enabled = true;
          if (closed != null)
          {
               sr.sprite = closed;
          }
          else
          {
               sr.enabled = true;
          }
     }
}
