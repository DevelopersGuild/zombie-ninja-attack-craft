using UnityEngine;
using System.Collections;

public class BasicAttack : MonoBehaviour
{
     private double t;
    // Use this for initialization
    void Start()
    {
         t = 0.7;
    }

    // Update is called once per frame
    void Update()
    {
         if (t <= 0)
         {
             // Destroy(gameObject);
         }
         t -= Time.deltaTime;
    }

    void DestroySelf()
    {
        //animation
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Health hp = other.gameObject.GetComponent<Health>();
            hp.CalculateKnockback(other.collider, transform.position);
            hp.TakeDamage(1);
            Player pl = other.gameObject.GetComponent<Player>();
            pl.isInvincible = true;
        }
    }
}
