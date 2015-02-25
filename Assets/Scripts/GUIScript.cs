using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

    public Player player;
    public UILabel healthLabel;

    EnemyHealth playerHealth;

	// Use this for initialization
	void Start () {
        playerHealth = player.GetComponent<EnemyHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        healthLabel.text = playerHealth.currentHealth.ToString();
	}
}
