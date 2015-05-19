using UnityEngine;
using System.Collections;

public class FireChains : MonoBehaviour {
    public Fireballs fireball;
    private Fireballs fireballInstance;
    public int numChains;
    public int length;
    public float speed;
    public float spacing;
    private float currentRotation;
    private float angleBetween;

	// Use this for initialization
	void Start () {
        angleBetween = 360 / numChains;

        float currentY = 0 ;
        for (int j = 0; j < numChains; j++) {
            for (int i = 0; i < length; i++) {
                Fireballs fireballInstance = Instantiate(fireball) as Fireballs;
                fireballInstance.transform.parent = gameObject.transform;
                fireballInstance.transform.position = new Vector2(gameObject.transform.position.x, currentY + spacing);
                currentY += spacing;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        // Rotate the gameobject
        currentRotation += speed;
        transform.eulerAngles = new Vector3(0, 0, currentRotation);

        if (currentRotation > 360) {
            currentRotation = 0;
        }
	}
}
