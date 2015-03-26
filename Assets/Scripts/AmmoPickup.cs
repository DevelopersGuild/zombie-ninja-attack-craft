using UnityEngine;
using System.Collections;

public class AmmoPickup : MonoBehaviour {

    public int ammoValue;

    private AttackController attackController;

	// Use this for initialization
	void Start () {
        ammoValue = Random.Range(1, 4);
	}
	
	// Update is called once per frame

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            attackController = other.gameObject.GetComponent<AttackController>();
            attackController.ammo += ammoValue;
            Destroy(gameObject);
        }
    }
}
