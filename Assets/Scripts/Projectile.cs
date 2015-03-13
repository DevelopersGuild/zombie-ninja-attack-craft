using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float projectileSpeed;
    public float angle;
    public float range;

    public Vector2 originalPosition;
    public Vector2 targetPosition;

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
        Debug.Log("COLLIDING WITH SOMETHING");
        if (other.gameObject.tag == "MapCollidable") {
            Destroy(gameObject);
            Debug.Log("COLLIDING WITH Wall");
        }
    }


    public void Shoot(float angle, Vector2 velocity) {
        originalPosition = transform.position;
        transform.eulerAngles = new Vector3(0, 0, angle);
        rigidbody2D.velocity = velocity * projectileSpeed;
        Debug.Log(rigidbody2D.velocity);
    }

}
