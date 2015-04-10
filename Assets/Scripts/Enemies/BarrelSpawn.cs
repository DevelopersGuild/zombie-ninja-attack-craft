using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarrelSpawn : MonoBehaviour {

    public float timeToExplode;
    private float currentTime;

    public DropLoot dropLoot;

	// Use this for initialization
	void Start () {
        dropLoot = GetComponent<DropLoot>();
	}
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;

        //If too much time passes, the barrel explodes and drops enemies
        if (currentTime >= timeToExplode) {
            dropLoot.DropItem();
            Destroy(gameObject);
        }

	}
}
