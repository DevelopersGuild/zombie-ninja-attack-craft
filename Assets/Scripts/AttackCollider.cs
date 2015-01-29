using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {

    public BoxCollider2D boxCollider;
    public bool enemyInRange;

	// Use this for initialization
	void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        enemyInRange = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Attackable")){
           // Debug.Log("ENTER");
            enemyInRange = true;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Attackable")) {
            //Debug.Log("STAY");
            enemyInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Attackable")) {
            //Debug.Log("STAY");
            enemyInRange = false;
        }
    }

    public bool EnemyInRange() {
        return enemyInRange;
    }
}
