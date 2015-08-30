using UnityEngine;
using System.Collections;

public class RandomBarrel : MonoBehaviour {

     public Sprite[] barrels;
     private SpriteRenderer sr;
     private Health health;
     public ParticleSystem whiteParticle;
     public ParticleSystem brownParticle;

	// Use this for initialization
	void Start () {
          // Set a random barrel sprite
          sr = GetComponent<SpriteRenderer>();
          health = GetComponent<Health>();
          int random = Random.Range(0, barrels.Length);
          sr.sprite = barrels[random];

          // Change death particle accordingly
          if(random == 1 || random == 3)
          {
               health.deathParticle = whiteParticle;
          }else{
               health.deathParticle = brownParticle;
          }

;	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
