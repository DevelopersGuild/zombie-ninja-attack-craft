using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
     public class ElectricWall : MonoBehaviour
     {

          public float TimeToLive;
          private float knockback;
          public fakeElectricWall fakeWall;

          public void Start()
          {

          }

          public void Update()
          {
               TimeToLive -= Time.deltaTime;

               if (TimeToLive > 0)
               {
                    float remainder = TimeToLive % .1f;
                    //GetComponent<Renderer>().enabled = remainder > .05f;
               }
               else
               {
                    fakeElectricWall fWall = Instantiate(fakeWall, transform.position, transform.rotation) as fakeElectricWall;
                    Destroy(gameObject);
               }
          }

          void OnTriggerEnter2D(Collider2D other)
          {
               //if not a boss (really just the electric boss has this) then deal damage + knockback
               //Only reason it doesn't just have a normal box collider
               //and instead has an isTrigger is because of the electric guy boss fight
               if (other.gameObject.tag != "Boss")
               {

                    if (other.CompareTag("Player"))
                    {
                         BoxCollider2D box = GetComponent<BoxCollider2D>();
                         GameObject obj = other.gameObject;
                         Health hp = other.GetComponent<Health>();
                         hp.CalculateKnockback(other, transform.position);
                         hp.TakeDamage(1);
                         hp.CalculateKnockback(box, transform.position);
                         Player plr = obj.GetComponent<Player>();
                         plr.isInvincible = true;
                    }

               }
          }


     }

}