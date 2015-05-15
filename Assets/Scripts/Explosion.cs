﻿using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    private CircleCollider2D collider;

	// Use this for initialization
	void Start () {
        collider = GetComponent<CircleCollider2D>();
	}

    void DestroySelf() {
        Destroy(gameObject);
    }

    void disableCollider() {
        collider.enabled = false;
    }
}
