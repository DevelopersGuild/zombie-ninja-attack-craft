using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BarrelSpawn : MonoBehaviour {

    public float timeToExplode;
    private float currentTime;

    private DropLoot dropLoot;
    private Tween Shake;
    private bool hasShaken;

	// Use this for initialization
	void Start () {
        dropLoot = GetComponent<DropLoot>();

	}
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
          if(isActiveAndEnabled && !hasShaken)
          {
               Shake = transform.DOShakePosition(timeToExplode, new Vector3(.3f, .3f, 0), 10, 10);
               hasShaken = true;
          }

        //If too much time passes, the barrel explodes and drops enemies
        if (currentTime >= timeToExplode) {
            Shake.Kill(true);
            dropLoot.DropItem();
            Destroy(gameObject);
        }

	}

     void OnDestroy()
     {
          Shake.Kill(true);
     }
}
