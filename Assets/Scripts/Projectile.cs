using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float projectileSpeed;
    public float angle;
    public float range;

    public Vector2 originalPosition;
    public Vector2 targetPosition;
    public Vector2 currentVelocity;

    public void Update() {
        if(transform.position.x < originalPosition.x - range){
            Destroy(gameObject);
        }
        if (transform.position.x > originalPosition.x + range) {
            Destroy(gameObject);
        }
        if (transform.position.y < originalPosition.y - range) {
            Destroy(gameObject);
        }
        if (transform.position.y > originalPosition.y + range) {
            Destroy(gameObject);
        }
    }


    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "MapCollidable") {
            Destroy(gameObject);
        }
    }


    public void Shoot(float angle, Vector2 velocity) {
        originalPosition = transform.position;
        transform.eulerAngles = new Vector3(0, 0, angle);
        currentVelocity = velocity;
        rigidbody2D.velocity = currentVelocity * projectileSpeed;
    }

}
