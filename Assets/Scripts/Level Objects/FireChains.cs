using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireChains : MonoBehaviour {
    public Fireballs fireball;
    private Fireballs fireballInstance;
    public int numChains;
    public int length;
    public float speed;
    public float spacing;
    private float currentRotation;
    private float angleBetween;
    private List<Fireballs> fireballList = new List<Fireballs>();

	// Create the fire chain
	void Start () {
        // Initialize variables
        float height = 0.12f;
        angleBetween = 360 / numChains;

        float currentY = 0;
        float currentAngle = 0;
        for (int j = 0; j < numChains; j++) {
            // Create the fireballs and place them so they arent relative to the rotation
            for (int i = 0; i < length; i++) {
                fireballInstance = Instantiate(fireball) as Fireballs;
                fireballList.Add(fireballInstance);
                fireballInstance.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + currentY + spacing + height);
                currentY = currentY + height + spacing;
            }

            // Parent the fireballs to the chain so the fireballs rotate with the chain
            for (int k = 0; k < fireballList.Count; k++) {
                fireballList[k].transform.parent = gameObject.transform;
            }
            fireballList.Clear();

            // Rotate the parent object 
            currentY = 0;
            currentAngle += angleBetween;
            transform.eulerAngles = new Vector3(0, 0, currentAngle);
        }
	}
	
	// Rotate the firechain every frame
	void Update () {
        // Rotate the gameobject
        currentRotation += speed;
        transform.eulerAngles = new Vector3(0, 0, currentRotation);

        if (currentRotation > 360) {
            currentRotation = 0;
        }
	}
}
