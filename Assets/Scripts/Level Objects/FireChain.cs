using UnityEngine;
using System.Collections;

public class FireChain : MonoBehaviour {

    public float speed;
    private float rotation;

	// Update is called once per frame
	void FixedUpdate () {
        rotation += speed;
        transform.eulerAngles = new Vector3(0, 0, rotation);

        if (rotation > 360) {
            rotation = 0;
        }
	}
}
