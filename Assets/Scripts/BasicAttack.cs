using UnityEngine;
using System.Collections;

public class BasicAttack : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
