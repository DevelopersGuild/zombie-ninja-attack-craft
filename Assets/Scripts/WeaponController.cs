using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public Weapon currentWeapon;
    Transform transform;

	// Use this for initialization
	void Awake () {
        transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void Attack() {
        Instantiate(currentWeapon, transform.position, Quaternion.identity);
    }

    void SetWeapon(Weapon weapon) {
        currentWeapon = weapon;
    }

    Weapon GetWeapon() {
        return currentWeapon;
    }
}
