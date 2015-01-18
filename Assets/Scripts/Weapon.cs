using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public float damage;
    public float speed;
    public float colliderSizeX;
    public float colliderSizeY;
    [SerializeField]
    public enum SwingTypes { stab, swing, range };

	// Use this for initialization
	void Start () {
        int swingType = (int) SwingTypes.stab;
        colliderSizeX = GetComponentInChildren<BoxCollider2D>().size.x;
        colliderSizeY = GetComponentInChildren<BoxCollider2D>().size.y;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attack() {

    }
}
