using UnityEngine;
using System.Collections;

public class LightningField : MonoBehaviour
{
     public Indicator indicObj, indic;
     private float radius;
     public Player player;

     private float time, cd;
     private Vector2 position;
     private Vector2 circle;

     // Use this for initialization
     void Start()
     {
          time = 10;
          player = FindObjectOfType<Player>();
     }

     public void set(float r, float cooldown)
     {
          cd = cooldown;
          radius = r;
     }

     // Update is called once per frame
     void Update()
     {
          if (time < 0)
          {
               Destroy(gameObject);
          }

          if (cd < 0)
          {
               circle = Random.insideUnitCircle * radius;
               position = new Vector2(player.transform.position.x + circle.x, player.transform.position.y + circle.y);
               indic = Instantiate(indicObj, position, transform.rotation) as Indicator;
               indic.set(1);
               cd = 3;
          }
          cd -= Time.deltaTime;
          time -= Time.deltaTime;

     }

     void DestroySelf()
     {
          //animation
          Destroy(gameObject);
     }

}
