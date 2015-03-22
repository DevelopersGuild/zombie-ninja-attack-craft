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
        if(other.CompareTag("Attackable")){
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Attackable")) {
            enemiesInRange.Remove(other.gameObject);
        }
    }

}
