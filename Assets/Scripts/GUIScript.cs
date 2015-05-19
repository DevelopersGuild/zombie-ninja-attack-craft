using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

    public Player player;
    public UILabel healthLabel;
    public UILabel scoreLabel;
    public UILabel ammoLabel;


    Health playerHealth;
    AttackController playerAttackController;

	// Use this for initialization
	void Start () {
        playerHealth = player.GetComponent<Health>();
        playerAttackController = player.GetComponent<AttackController>();
	}
	
	// Update is called once per frame
	void Update () {
        healthLabel.text = playerHealth.currentHealth.ToString();
        scoreLabel.text = GameManager.getCoins().ToString();
        ammoLabel.text = playerAttackController.ammo.ToString();
	}
}
