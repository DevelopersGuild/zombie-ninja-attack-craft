using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;


    public class Projectile : MonoBehaviour {

        public float projectileSpeed;
        public float angle;

        public float stun;
        private bool homing, shot;

        public float TimeToLive;

        private Vector2 PlayerPos, direction;
        private Player player;
        public Vector2 originalPosition;
        public Vector2 targetPosition;
        public Vector2 currentVelocity;

        public void Start() {
            Destroy(transform.gameObject, TimeToLive);
            homing = shot = false;
            player = FindObjectOfType<Player>();
        }

        public void Update() {
            if (AutoTileMap.Instance.GetAutotileCollisionAtPosition(transform.position) == AutoTileMap.eTileCollisionType.BLOCK) {
                Destroy(transform.gameObject);
            }
            if (shot)
            {
                if (homing)
                {
                    PlayerPos = player.transform.position;
                    direction = new Vector2(PlayerPos.x - transform.position.x, PlayerPos.y - transform.position.y);
                    GetComponent<Rigidbody2D>().velocity += direction.normalized * 0.2f;
                }
            }
        }

        public void setStun(float st)
        {
            stun = st;

        }

        public void home(bool x)
        {
            homing = x;
        }


        public void Shoot(float angle, Vector2 velocity) {
            originalPosition = transform.position;
            transform.eulerAngles = new Vector3(0, 0, angle);
            currentVelocity = velocity;
            GetComponent<Rigidbody2D>().velocity = currentVelocity * projectileSpeed;
            shot = true;
        }

        public float getStun()
        {
            return stun;
        }

   

    }

