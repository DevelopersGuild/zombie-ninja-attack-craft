using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;


    public class Projectile : MonoBehaviour {

        public float projectileSpeed;
        public float angle;
        public float range;

        public float TimeToLive;

        public Vector2 originalPosition;
        public Vector2 targetPosition;
        public Vector2 currentVelocity;

        public void Start() {
            Destroy(transform.gameObject, TimeToLive);
        }

        public void Update() {
            if (AutoTileMap.Instance.GetAutotileCollisionAtPosition(transform.position) == AutoTileMap.eTileCollisionType.BLOCK) {
                Destroy(transform.gameObject);
            }
        }

        public void Shoot(float angle, Vector2 velocity) {
            originalPosition = transform.position;
            transform.eulerAngles = new Vector3(0, 0, angle);
            currentVelocity = velocity;
            GetComponent<Rigidbody2D>().velocity = currentVelocity * projectileSpeed;
        }

    }

