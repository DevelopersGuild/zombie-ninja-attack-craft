using UnityEngine;
using System.Collections;

public class ArrowTrap : MonoBehaviour {

    public Projectile TrapArrow;
    private SpriteRenderer sr;
    private Transform parent;
    public Sprite facingDown;
    public Sprite facingRight;

    public enum ShootDirections { up, down, left, right}
    public ShootDirections direction;

	// Use this for initialization
	void Start () {
        // Components of parent
        sr = GetComponentInParent<SpriteRenderer>();
        parent = transform.parent;

        ///Set the rotation/direction of the trigger collider depending on where the arrow will shoot
        if (direction == ShootDirections.up) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction == ShootDirections.down) {
            sr.sprite = facingDown;
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (direction == ShootDirections.right) {
            sr.sprite = facingRight;
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
        else if (direction == ShootDirections.left) {
            sr.sprite = facingRight;
            transform.eulerAngles = new Vector3(0, 0, 270);
            parent.localScale = new Vector3(-1, 1, 1);
        }

	}

    void OnTriggerEnter2D(Collider2D other) {
        //If the collider is triggered by a player, shoot an arrow in the direction that the player triggered the collider
        if (other.gameObject.tag == "Player") {
            if (direction == ShootDirections.up) {
                Projectile projectile = Instantiate(TrapArrow, new Vector2(transform.position.x, transform.position.y + 0.25f), transform.rotation) as Projectile;
                projectile.Shoot(90, new Vector2(0, 1));
            }
            else if (direction == ShootDirections.down) {
                Projectile projectile = Instantiate(TrapArrow, new Vector2(transform.position.x, transform.position.y - 0.25f), transform.rotation) as Projectile;
                projectile.Shoot(-90, new Vector2(0, -1));
            }
            else if (direction == ShootDirections.right) {
                Projectile projectile = Instantiate(TrapArrow, new Vector2(transform.position.x + 0.25f, transform.position.y), transform.rotation) as Projectile;
                projectile.Shoot(180, new Vector2(1, 0));
            }
            else if (direction == ShootDirections.left) {
                Projectile projectile = Instantiate(TrapArrow, new Vector2(transform.position.x - 0.25f, transform.position.y), transform.rotation) as Projectile;
                projectile.Shoot(0, new Vector2(-1, 0));
            }
        }
    }
}
