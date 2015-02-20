using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {

    public BoxCollider2D boxCollider;
    public bool enemyInRange;
    public ArrayList enemiesInRange = new ArrayList();

	// Use this for initialization
	void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        enemyInRange = false;
	}

    void Update() {
        //Debug.Log("enemysize" + enemiesInRange.Count);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Attackable") || other.CompareTag ("Cuttable")){
            enemiesInRange.Add(other);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
		if (other.CompareTag("Attackable")|| other.CompareTag ("Cuttable")) {

        }
    }

    void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Attackable")|| other.CompareTag ("Cuttable")) {
            enemiesInRange.Remove(other);
        }
    }

}
