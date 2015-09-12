using UnityEngine;
using System.Collections;

public class ArrowTower : MonoBehaviour
{
     public Projectile arrow;
     private Player player;
     private Health playerHealth;
     private Transform playerPosition;

     public Projectile arrowObj;

     public float throwForce;

     private bool isAggroed;
     public float throwCooldown;
     private float currentTime;


     // Update is called once per frame
     public void Start()
     {
          player = FindObjectOfType<Player>();
          playerHealth = player.GetComponent<Health>();
          playerPosition = player.transform;
          isAggroed = true;
     }
     void Update()
     {
          if (isAggroed && playerHealth.isDead == false)
          {
               //Throw a bomb with a cooldown towards the direction of the player if the player is within aggro range
               currentTime += Time.deltaTime;
               if (currentTime >= throwCooldown)
               {
                    arrowObj = Instantiate(arrow, transform.position, transform.rotation) as Projectile;
                    Vector3 dir = player.transform.position - transform.position;
                    dir = player.transform.InverseTransformDirection(dir);
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    arrowObj.Shoot(angle, dir.normalized * throwForce);
                    //arrowObj.GetComponent<Rigidbody2D>().velocity = (toPlayer * throwForce);
                    currentTime = 0;
               }
          }
     }




}
